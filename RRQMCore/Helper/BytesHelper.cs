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
using System.Collections.Generic;

namespace RRQMCore.Helper
{
    /// <summary>
    /// 字节数组扩展类
    /// </summary>
    public static class BytesHelper
    {
        /// <summary>
        /// 索引包含数组
        /// </summary>
        /// <param name="srcByteArray"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <param name="subByteArray"></param>
        /// <returns></returns>
        public static List<int> IndexOfInclude(this byte[] srcByteArray, int offset, int length, byte[] subByteArray)
        {
            int subByteArrayLen = subByteArray.Length;
            List<int> indexes = new List<int>();
            if (length < subByteArrayLen)
            {
                return indexes;
            }
            int hitLength = 0;
            for (int i = offset; i < length; i++)
            {
                if (srcByteArray[i] == subByteArray[hitLength])
                {
                    hitLength++;
                }
                else
                {
                    hitLength = 0;
                }

                if (hitLength == subByteArray.Length)
                {
                    hitLength = 0;
                    indexes.Add(i);
                }
            }

            return indexes;
        }

        /// <summary>
        /// 索引第一个包含数组
        /// </summary>
        /// <param name="srcByteArray"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <param name="subByteArray"></param>
        /// <returns></returns>
        public static int IndexOfFirst(this byte[] srcByteArray, int offset, int length, byte[] subByteArray)
        {
            if (length < subByteArray.Length)
            {
                return -1;
            }
            int hitLength = 0;
            for (int i = offset; i < length; i++)
            {
                if (srcByteArray[i] == subByteArray[hitLength])
                {
                    hitLength++;
                }
                else
                {
                    hitLength = 0;
                }

                if (hitLength == subByteArray.Length)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// 转Base64。
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ToBase64(this byte[] data)
        {
            return Convert.ToBase64String(data);
        }
    }
}