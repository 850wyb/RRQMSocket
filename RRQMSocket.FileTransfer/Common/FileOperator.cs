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

namespace RRQMSocket.FileTransfer
{
    /// <summary>
    /// 文件传输操作器
    /// </summary>
    public class FileOperator : StreamOperator
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FileOperator() : base()
        {
        }

        internal void AddFileFlow(int flow, long length)
        {
            this.speedTemp += flow;
            this.completedLength += flow;
            this.progress = (float)((double)this.completedLength / length);
        }

        internal void SetFileCompletedLength(long completedLength)
        {
            this.completedLength = completedLength;
        }

        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        internal Result SetFileResult(Result result)
        {
            this.result = result;
            return result;
        }
    }
}