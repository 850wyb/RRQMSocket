//------------------------------------------------------------------------------
//  此代码版权（除特别声明或在RRQMCore.XREF命名空间的代码）归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  Gitee源代码仓库：https://gitee.com/RRQM_Home
//  Github源代码仓库：https://github.com/RRQM
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
using RRQMCore.Collections.Concurrent;

namespace RRQMSocket
{
    /// <summary>
    /// 传输字节
    /// </summary>
    public struct TransferByte : IQueueData
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        public TransferByte(byte[] buffer, int offset, int length)
        {
            this.offset = offset;
            this.length = length;
            this.buffer = buffer;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="buffer"></param>
        public TransferByte(byte[] buffer) : this(buffer, 0, buffer.Length)
        {
        }

        private int offset;
        private int length;
        private byte[] buffer;

        /// <summary>
        /// 数据内存
        /// </summary>
        public byte[] Buffer => this.buffer;

        /// <summary>
        /// 偏移
        /// </summary>
        public int Offset => this.offset;

        /// <summary>
        /// 长度
        /// </summary>
        public int Length => this.length;

        /// <summary>
        /// 尺寸
        /// </summary>
        public int Size => length;
    }
}