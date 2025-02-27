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

#region License

// Copyright (c) 2007 James Newton-King
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

#endregion License

using RRQMCore.XREF.Newtonsoft.Json.Utilities;

namespace RRQMCore.XREF.Newtonsoft.Json.Serialization
{
    /// <summary>
    /// A camel case naming strategy.
    /// </summary>
    public class CamelCaseNamingStrategy : NamingStrategy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CamelCaseNamingStrategy"/> class.
        /// </summary>
        /// <param name="processDictionaryKeys">
        /// A flag indicating whether dictionary keys should be processed.
        /// </param>
        /// <param name="overrideSpecifiedNames">
        /// A flag indicating whether explicitly specified property names should be processed,
        /// e.g. a property name customized with a <see cref="JsonPropertyAttribute"/>.
        /// </param>
        public CamelCaseNamingStrategy(bool processDictionaryKeys, bool overrideSpecifiedNames)
        {
            ProcessDictionaryKeys = processDictionaryKeys;
            OverrideSpecifiedNames = overrideSpecifiedNames;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CamelCaseNamingStrategy"/> class.
        /// </summary>
        /// <param name="processDictionaryKeys">
        /// A flag indicating whether dictionary keys should be processed.
        /// </param>
        /// <param name="overrideSpecifiedNames">
        /// A flag indicating whether explicitly specified property names should be processed,
        /// e.g. a property name customized with a <see cref="JsonPropertyAttribute"/>.
        /// </param>
        /// <param name="processExtensionDataNames">
        /// A flag indicating whether extension data names should be processed.
        /// </param>
        public CamelCaseNamingStrategy(bool processDictionaryKeys, bool overrideSpecifiedNames, bool processExtensionDataNames)
            : this(processDictionaryKeys, overrideSpecifiedNames)
        {
            ProcessExtensionDataNames = processExtensionDataNames;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CamelCaseNamingStrategy"/> class.
        /// </summary>
        public CamelCaseNamingStrategy()
        {
        }

        /// <summary>
        /// Resolves the specified property name.
        /// </summary>
        /// <param name="name">The property name to resolve.</param>
        /// <returns>The resolved property name.</returns>
        protected override string ResolvePropertyName(string name)
        {
            return StringUtils.ToCamelCase(name);
        }
    }
}