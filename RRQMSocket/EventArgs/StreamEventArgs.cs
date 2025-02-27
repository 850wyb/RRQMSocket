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
using RRQMCore;
using System.IO;

namespace RRQMSocket
{
    /// <summary>
    /// 流事件参数
    /// </summary>
    public class StreamEventArgs : MesEventArgs
    {
        private Metadata metadata;

        private StreamInfo streamInfo;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="streamInfo"></param>
        public StreamEventArgs(Metadata metadata, StreamInfo streamInfo)
        {
            this.metadata = metadata;
            this.streamInfo = streamInfo;
        }

        /// <summary>
        /// 用于接收流的容器
        /// </summary>
        public Stream Bucket { get; set; }

        /// <summary>
        /// 用于可传输的元数据
        /// </summary>
        public Metadata Metadata
        {
            get { return metadata; }
        }

        /// <summary>
        /// 流信息
        /// </summary>
        public StreamInfo StreamInfo
        {
            get { return streamInfo; }
        }
    }
}