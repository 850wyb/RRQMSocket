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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRQMCore.Helper
{
    /// <summary>
    /// Byte辅助扩展类
    /// </summary>
    public static class ByteHelper
    {
        /// <summary>
        /// 获取字节中的指定Bit的值
        /// </summary>
        /// <param name="this">字节</param>
        /// <param name="index">Bit的索引值(0-7)</param>
        /// <returns></returns>
        public static int GetBit(this byte @this, short index)
        {
            byte x = 1;
            switch (index)
            {
                case 0: { x = 0x01; } break;
                case 1: { x = 0x02; } break;
                case 2: { x = 0x04; } break;
                case 3: { x = 0x08; } break;
                case 4: { x = 0x10; } break;
                case 5: { x = 0x20; } break;
                case 6: { x = 0x40; } break;
                case 7: { x = 0x80; } break;
                default: { return 0; }
            }
            return (@this & x) == x ? 1 : 0;
        }

        /// <summary>
        /// 设置字节中的指定Bit的值
        /// </summary>
        /// <param name="this">字节</param>
        /// <param name="index">Bit的索引值(0-7)</param>
        /// <param name="bitvalue">Bit值(0,1)</param>
        /// <returns></returns>
        public static byte SetBit(this byte @this, short index, int bitvalue)
        {
            var _byte = @this;
            if (bitvalue == 1)
            {
                switch (index)
                {
                    case 0: { return _byte |= 0x01; }
                    case 1: { return _byte |= 0x02; }
                    case 2: { return _byte |= 0x04; }
                    case 3: { return _byte |= 0x08; }
                    case 4: { return _byte |= 0x10; }
                    case 5: { return _byte |= 0x20; }
                    case 6: { return _byte |= 0x40; }
                    case 7: { return _byte |= 0x80; }
                    default: { return _byte; }
                }
            }
            else
            {
                switch (index)
                {
                    case 0: { return _byte &= 0xFE; }
                    case 1: { return _byte &= 0xFD; }
                    case 2: { return _byte &= 0xFB; }
                    case 3: { return _byte &= 0xF7; }
                    case 4: { return _byte &= 0xEF; }
                    case 5: { return _byte &= 0xDF; }
                    case 6: { return _byte &= 0xBF; }
                    case 7: { return _byte &= 0x7F; }
                    default: { return _byte; }
                }
            }
        }
    }
}
