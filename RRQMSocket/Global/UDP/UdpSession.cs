//------------------------------------------------------------------------------
//  此代码版权归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  源代码仓库：https://gitee.com/RRQM_Home
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using RRQMCore.ByteManager;
using RRQMCore.Exceptions;
using RRQMCore.Log;
using RRQMCore.Pool;

namespace RRQMSocket
{
    /// <summary>
    /// TCP服务器
    /// </summary>
    public abstract class UdpSession : BaseSocket, IService, IClient, IHandleBuffer
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UdpSession() : this(new BytePool(1024 * 1024 * 1000, 1024 * 1024 * 20))
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="bytePool"></param>
        public UdpSession(BytePool bytePool) : base(bytePool)
        {
            this.BufferLength = 1024 * 64;
        }

        /// <summary>
        /// 获取绑定状态
        /// </summary>
        public bool IsBind { get; private set; }

        /// <summary>
        /// 默认远程节点
        /// </summary>
        public EndPoint DefaultRemotePoint { get; set; }

        /// <summary>
        /// 已接收数据次数
        /// </summary>
        public long RecivedCount { get { return this.recivedCount; } }

        private BufferQueueGroup[] bufferQueueGroups;
        private SocketAsyncEventArgs recvEventArg;
        private long recivedCount;

      

        /// <summary>
        /// 绑定UDP服务
        /// </summary>
        /// <param name="addressFamily">寻址方案，支持IPv6</param>
        /// <param name="endPoint">节点</param>
        /// <param name="threadCount">多线程数量</param>
        /// <exception cref="RRQMException"></exception>
        public void Bind(AddressFamily addressFamily, EndPoint endPoint, int threadCount)
        {
            if (this.disposable)
            {
                throw new RRQMException("无法重新利用已释放对象");
            }
            if (threadCount < 1)
            {
                throw new RRQMException("逻辑线程数量不能小于1");
            }
            if (!IsBind)
            {
                try
                {
                    Socket socket = new Socket(addressFamily, SocketType.Dgram, ProtocolType.Udp);
                    PreviewBind(socket);
                    socket.Bind(endPoint);
                    this.MainSocket = socket;

                    this.recvEventArg = new SocketAsyncEventArgs();
                    this.recvEventArg.Completed += this.RecvEventArg_Completed;
                    ByteBlock byteBlock = this.BytePool.GetByteBlock(this.BufferLength);
                    this.recvEventArg.UserToken = byteBlock;
                    this.recvEventArg.SetBuffer(byteBlock.Buffer, 0, byteBlock.Buffer.Length);
                    this.recvEventArg.RemoteEndPoint = endPoint;
                    this.MainSocket.ReceiveFromAsync(this.recvEventArg);
                }
                catch (Exception e)
                {
                    throw new RRQMException(e.Message);
                }

                bufferQueueGroups = new BufferQueueGroup[threadCount];
                for (int i = 0; i < threadCount; i++)
                {
                    BufferQueueGroup bufferQueueGroup = new BufferQueueGroup();
                    bufferQueueGroups[i] = bufferQueueGroup;
                    bufferQueueGroup.Thread = new Thread(Handle);//处理用户的消息
                    bufferQueueGroup.clientBufferPool = new ObjectPool<ClientBuffer>(10000);//处理用户的消息
                    bufferQueueGroup.waitHandleBuffer = new AutoResetEvent(false);
                    bufferQueueGroup.bufferAndClient = new BufferQueue();
                    bufferQueueGroup.Thread.IsBackground = true;
                    bufferQueueGroup.Thread.Name = i + "号服务器处理线程";
                    bufferQueueGroup.Thread.Start(bufferQueueGroup);
                }
            }
            else
            {
                throw new RRQMException("重复绑定");
            }

            IsBind = true;
        }

        /// <summary>
        /// 绑定服务
        /// </summary>
        /// <param name="port">端口号</param>
        /// <param name="threadCount">多线程数量</param>
        /// <exception cref="RRQMException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public void Bind(int port, int threadCount = 1)
        {
            IPHost iPHost = IPHost.CreatIPHost($"0.0.0.0:{port}");
            this.Bind(iPHost,threadCount);
        }

        /// <summary>
        /// 绑定服务
        /// </summary>
        /// <param name="iPHost">ip和端口号，格式如“127.0.0.1:7789”。IP可输入Ipv6</param>
        /// <param name="threadCount">多线程数量</param>
        /// <exception cref="RRQMException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public void Bind(IPHost iPHost, int threadCount)
        {
            if (iPHost==null)
            {
                throw new ArgumentNullException("iPHost不能为空。");
            }
            this.Bind(iPHost.AddressFamily,iPHost.EndPoint,threadCount);
        }

        /// <summary>
        /// 在Socket初始化对象后，Bind之前调用。
        /// 可用于设置Socket参数。
        /// 父类方法可覆盖。
        /// </summary>
        /// <param name="socket"></param>
        protected virtual void PreviewBind(Socket socket)
        {
        }

        private void RecvEventArg_Completed(object sender, SocketAsyncEventArgs e)
        {
            if (e.LastOperation == SocketAsyncOperation.ReceiveFrom)
            {
                ProcessReceive(e);
            }
        }

        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            if (!this.disposable)
            {
                if (this.recvEventArg.SocketError == SocketError.Success)
                {
                    ByteBlock byteBlock = (ByteBlock)e.UserToken;
                    byteBlock.Position = e.BytesTransferred;
                    byteBlock.SetLength(e.BytesTransferred);

                    BufferQueueGroup queueGroup = this.bufferQueueGroups[++this.recivedCount % this.bufferQueueGroups.Length];
                    ClientBuffer clientBuffer = queueGroup.clientBufferPool.GetObject();
                    clientBuffer.endPoint = e.RemoteEndPoint;
                    clientBuffer.byteBlock = byteBlock;
                    queueGroup.bufferAndClient.Enqueue(clientBuffer);
                    queueGroup.waitHandleBuffer.Set();
                    ByteBlock newByteBlock = this.BytePool.GetByteBlock(this.BufferLength);
                    e.UserToken = newByteBlock;
                    e.SetBuffer(newByteBlock.Buffer, 0, newByteBlock.Buffer.Length);
                    if (!this.MainSocket.ReceiveFromAsync(this.recvEventArg))
                    {
                        ProcessReceive(e);
                    }
                }
            }
        }

        private void Handle(object o)
        {
            BufferQueueGroup queueGroup = (BufferQueueGroup)o;
            while (true)
            {
                if (disposable)
                {
                    break;
                }
                ClientBuffer clientBuffer;
                if (queueGroup.bufferAndClient.TryDequeue(out clientBuffer))
                {
                    try
                    {
                        HandleBuffer(clientBuffer);
                    }
                    catch (Exception e)
                    {
                        Logger.Debug(LogType.Error, this, $"在处理数据时发生错误，信息：{e.Message}");
                    }
                    finally
                    {
                        queueGroup.clientBufferPool.DestroyObject(clientBuffer);
                        clientBuffer.byteBlock.Dispose();
                    }
                }
                else
                {
                    queueGroup.waitHandleBuffer.WaitOne();
                }
            }
        }

        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <param name="remoteEP"></param>
        public void SendTo(byte[] buffer, int offset, int length, EndPoint remoteEP)
        {
            this.MainSocket.SendTo(buffer, offset, length, SocketFlags.None, remoteEP);
        }

        /// <summary>
        /// 处理已接收到的数据
        /// </summary>
        /// <param name="remoteEndPoint"></param>
        /// <param name="byteBlock"></param>
        protected abstract void HandleReceivedData(EndPoint remoteEndPoint, ByteBlock byteBlock);

        /// <summary>
        /// 向默认终结点发送
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        public void Send(byte[] buffer, int offset, int length)
        {
            if (this.DefaultRemotePoint == null)
            {
                throw new RRQMException("默认终结点为空");
            }
            this.SendTo(buffer, offset, length, this.DefaultRemotePoint);
        }

        /// <summary>
        /// 向默认终结点发送
        /// </summary>
        /// <param name="buffer"></param>
        public void Send(byte[] buffer)
        {
            this.Send(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// 向默认终结点发送
        /// </summary>
        /// <param name="byteBlock"></param>
        public void Send(ByteBlock byteBlock)
        {
            this.Send(byteBlock.Buffer, 0, (int)byteBlock.Length);
        }

        /// <summary>
        /// 关闭服务器并释放服务器资源
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();

            foreach (var item in bufferQueueGroups)
            {
                item.Dispose();
            }
        }

        private void HandleBuffer(ClientBuffer clientBuffer)
        {
            HandleReceivedData(clientBuffer.endPoint, clientBuffer.byteBlock);
        }

        void IHandleBuffer.HandleBuffer(ClientBuffer clientBuffer)
        {
        }

        
    }
}