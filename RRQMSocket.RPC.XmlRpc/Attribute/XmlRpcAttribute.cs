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
using System;

namespace RRQMSocket.RPC.XmlRpc
{
    /// <summary>
    /// 适用于XmlRpc的标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class XmlRpcAttribute : RPCAttribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public XmlRpcAttribute()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="actionKey"></param>
        public XmlRpcAttribute(string actionKey)
        {
            this.ActionKey = actionKey;
        }

        /// <summary>
        /// 服务唯一标识
        /// </summary>
        public string ActionKey { get; private set; }
    }
}