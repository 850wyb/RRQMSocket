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

using System;
using System.IO;

#if HAVE_ASYNC
using System.Threading;
using System.Threading.Tasks;
#endif

using System.Collections.Generic;
using System.Diagnostics;

#if !HAVE_LINQ

using RRQMCore.XREF.Newtonsoft.Json.Utilities.LinqBridge;

#else
using System.Linq;
#endif

namespace RRQMCore.XREF.Newtonsoft.Json.Utilities
{
    internal static class BufferUtils
    {
        public static char[] RentBuffer(IArrayPool<char> bufferPool, int minSize)
        {
            if (bufferPool == null)
            {
                return new char[minSize];
            }

            char[] buffer = bufferPool.Rent(minSize);
            return buffer;
        }

        public static void ReturnBuffer(IArrayPool<char> bufferPool, char[] buffer)
        {
            bufferPool?.Return(buffer);
        }

        public static char[] EnsureBufferSize(IArrayPool<char> bufferPool, int size, char[] buffer)
        {
            if (bufferPool == null)
            {
                return new char[size];
            }

            if (buffer != null)
            {
                bufferPool.Return(buffer);
            }

            return bufferPool.Rent(size);
        }
    }

    internal static class JavaScriptUtils
    {
        internal static readonly bool[] SingleQuoteCharEscapeFlags = new bool[128];
        internal static readonly bool[] DoubleQuoteCharEscapeFlags = new bool[128];
        internal static readonly bool[] HtmlCharEscapeFlags = new bool[128];

        private const int UnicodeTextLength = 6;

        static JavaScriptUtils()
        {
            IList<char> escapeChars = new List<char>
            {
                '\n', '\r', '\t', '\\', '\f', '\b',
            };
            for (int i = 0; i < ' '; i++)
            {
                escapeChars.Add((char)i);
            }

            foreach (char escapeChar in escapeChars.Union(new[] { '\'' }))
            {
                SingleQuoteCharEscapeFlags[escapeChar] = true;
            }
            foreach (char escapeChar in escapeChars.Union(new[] { '"' }))
            {
                DoubleQuoteCharEscapeFlags[escapeChar] = true;
            }
            foreach (char escapeChar in escapeChars.Union(new[] { '"', '\'', '<', '>', '&' }))
            {
                HtmlCharEscapeFlags[escapeChar] = true;
            }
        }

        private const string EscapedUnicodeText = "!";

        public static bool[] GetCharEscapeFlags(StringEscapeHandling stringEscapeHandling, char quoteChar)
        {
            if (stringEscapeHandling == StringEscapeHandling.EscapeHtml)
            {
                return HtmlCharEscapeFlags;
            }

            if (quoteChar == '"')
            {
                return DoubleQuoteCharEscapeFlags;
            }

            return SingleQuoteCharEscapeFlags;
        }

        public static bool ShouldEscapeJavaScriptString(string s, bool[] charEscapeFlags)
        {
            if (s == null)
            {
                return false;
            }

            foreach (char c in s)
            {
                if (c >= charEscapeFlags.Length || charEscapeFlags[c])
                {
                    return true;
                }
            }

            return false;
        }

        public static void WriteEscapedJavaScriptString(TextWriter writer, string s, char delimiter, bool appendDelimiters,
            bool[] charEscapeFlags, StringEscapeHandling stringEscapeHandling, IArrayPool<char> bufferPool, ref char[] writeBuffer)
        {
            // leading delimiter
            if (appendDelimiters)
            {
                writer.Write(delimiter);
            }

            if (!string.IsNullOrEmpty(s))
            {
                int lastWritePosition = FirstCharToEscape(s, charEscapeFlags, stringEscapeHandling);
                if (lastWritePosition == -1)
                {
                    writer.Write(s);
                }
                else
                {
                    if (lastWritePosition != 0)
                    {
                        if (writeBuffer == null || writeBuffer.Length < lastWritePosition)
                        {
                            writeBuffer = BufferUtils.EnsureBufferSize(bufferPool, lastWritePosition, writeBuffer);
                        }

                        // write unchanged chars at start of text.
                        s.CopyTo(0, writeBuffer, 0, lastWritePosition);
                        writer.Write(writeBuffer, 0, lastWritePosition);
                    }

                    int length;
                    for (int i = lastWritePosition; i < s.Length; i++)
                    {
                        char c = s[i];

                        if (c < charEscapeFlags.Length && !charEscapeFlags[c])
                        {
                            continue;
                        }

                        string escapedValue;

                        switch (c)
                        {
                            case '\t':
                                escapedValue = @"\t";
                                break;

                            case '\n':
                                escapedValue = @"\n";
                                break;

                            case '\r':
                                escapedValue = @"\r";
                                break;

                            case '\f':
                                escapedValue = @"\f";
                                break;

                            case '\b':
                                escapedValue = @"\b";
                                break;

                            case '\\':
                                escapedValue = @"\\";
                                break;

                            case '\u0085': // Next Line
                                escapedValue = @"\u0085";
                                break;

                            case '\u2028': // Line Separator
                                escapedValue = @"\u2028";
                                break;

                            case '\u2029': // Paragraph Separator
                                escapedValue = @"\u2029";
                                break;

                            default:
                                if (c < charEscapeFlags.Length || stringEscapeHandling == StringEscapeHandling.EscapeNonAscii)
                                {
                                    if (c == '\'' && stringEscapeHandling != StringEscapeHandling.EscapeHtml)
                                    {
                                        escapedValue = @"\'";
                                    }
                                    else if (c == '"' && stringEscapeHandling != StringEscapeHandling.EscapeHtml)
                                    {
                                        escapedValue = @"\""";
                                    }
                                    else
                                    {
                                        if (writeBuffer == null || writeBuffer.Length < UnicodeTextLength)
                                        {
                                            writeBuffer = BufferUtils.EnsureBufferSize(bufferPool, UnicodeTextLength, writeBuffer);
                                        }

                                        StringUtils.ToCharAsUnicode(c, writeBuffer);

                                        // slightly hacky but it saves multiple conditions in if test
                                        escapedValue = EscapedUnicodeText;
                                    }
                                }
                                else
                                {
                                    escapedValue = null;
                                }
                                break;
                        }

                        if (escapedValue == null)
                        {
                            continue;
                        }

                        bool isEscapedUnicodeText = string.Equals(escapedValue, EscapedUnicodeText);

                        if (i > lastWritePosition)
                        {
                            length = i - lastWritePosition + ((isEscapedUnicodeText) ? UnicodeTextLength : 0);
                            int start = (isEscapedUnicodeText) ? UnicodeTextLength : 0;

                            if (writeBuffer == null || writeBuffer.Length < length)
                            {
                                char[] newBuffer = BufferUtils.RentBuffer(bufferPool, length);

                                // the unicode text is already in the buffer
                                // copy it over when creating new buffer
                                if (isEscapedUnicodeText)
                                {
                                    Debug.Assert(writeBuffer != null, "Write buffer should never be null because it is set when the escaped unicode text is encountered.");

                                    Array.Copy(writeBuffer, newBuffer, UnicodeTextLength);
                                }

                                BufferUtils.ReturnBuffer(bufferPool, writeBuffer);

                                writeBuffer = newBuffer;
                            }

                            s.CopyTo(lastWritePosition, writeBuffer, start, length - start);

                            // write unchanged chars before writing escaped text
                            writer.Write(writeBuffer, start, length - start);
                        }

                        lastWritePosition = i + 1;
                        if (!isEscapedUnicodeText)
                        {
                            writer.Write(escapedValue);
                        }
                        else
                        {
                            writer.Write(writeBuffer, 0, UnicodeTextLength);
                        }
                    }

                    Debug.Assert(lastWritePosition != 0);
                    length = s.Length - lastWritePosition;
                    if (length > 0)
                    {
                        if (writeBuffer == null || writeBuffer.Length < length)
                        {
                            writeBuffer = BufferUtils.EnsureBufferSize(bufferPool, length, writeBuffer);
                        }

                        s.CopyTo(lastWritePosition, writeBuffer, 0, length);

                        // write remaining text
                        writer.Write(writeBuffer, 0, length);
                    }
                }
            }

            // trailing delimiter
            if (appendDelimiters)
            {
                writer.Write(delimiter);
            }
        }

        public static string ToEscapedJavaScriptString(string value, char delimiter, bool appendDelimiters, StringEscapeHandling stringEscapeHandling)
        {
            bool[] charEscapeFlags = GetCharEscapeFlags(stringEscapeHandling, delimiter);

            using (StringWriter w = StringUtils.CreateStringWriter(value?.Length ?? 16))
            {
                char[] buffer = null;
                WriteEscapedJavaScriptString(w, value, delimiter, appendDelimiters, charEscapeFlags, stringEscapeHandling, null, ref buffer);
                return w.ToString();
            }
        }

        private static int FirstCharToEscape(string s, bool[] charEscapeFlags, StringEscapeHandling stringEscapeHandling)
        {
            for (int i = 0; i != s.Length; i++)
            {
                char c = s[i];

                if (c < charEscapeFlags.Length)
                {
                    if (charEscapeFlags[c])
                    {
                        return i;
                    }
                }
                else if (stringEscapeHandling == StringEscapeHandling.EscapeNonAscii)
                {
                    return i;
                }
                else
                {
                    switch (c)
                    {
                        case '\u0085':
                        case '\u2028':
                        case '\u2029':
                            return i;
                    }
                }
            }

            return -1;
        }

#if HAVE_ASYNC
        public static Task WriteEscapedJavaScriptStringAsync(TextWriter writer, string s, char delimiter, bool appendDelimiters, bool[] charEscapeFlags, StringEscapeHandling stringEscapeHandling, JsonTextWriter client, char[] writeBuffer, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return cancellationToken.FromCanceled();
            }

            if (appendDelimiters)
            {
                return WriteEscapedJavaScriptStringWithDelimitersAsync(writer, s, delimiter, charEscapeFlags, stringEscapeHandling, client, writeBuffer, cancellationToken);
            }

            if (string.IsNullOrEmpty(s))
            {
                return cancellationToken.CancelIfRequestedAsync() ?? AsyncUtils.CompletedTask;
            }

            return WriteEscapedJavaScriptStringWithoutDelimitersAsync(writer, s, charEscapeFlags, stringEscapeHandling, client, writeBuffer, cancellationToken);
        }

        private static Task WriteEscapedJavaScriptStringWithDelimitersAsync(TextWriter writer, string s, char delimiter,
            bool[] charEscapeFlags, StringEscapeHandling stringEscapeHandling, JsonTextWriter client, char[] writeBuffer, CancellationToken cancellationToken)
        {
            Task task = writer.WriteAsync(delimiter, cancellationToken);
            if (!task.IsCompletedSucessfully())
            {
                return WriteEscapedJavaScriptStringWithDelimitersAsync(task, writer, s, delimiter, charEscapeFlags, stringEscapeHandling, client, writeBuffer, cancellationToken);
            }

            if (!string.IsNullOrEmpty(s))
            {
                task = WriteEscapedJavaScriptStringWithoutDelimitersAsync(writer, s, charEscapeFlags, stringEscapeHandling, client, writeBuffer, cancellationToken);
                if (task.IsCompletedSucessfully())
                {
                    return writer.WriteAsync(delimiter, cancellationToken);
                }
            }

            return WriteCharAsync(task, writer, delimiter, cancellationToken);
        }

        private static async Task WriteEscapedJavaScriptStringWithDelimitersAsync(Task task, TextWriter writer, string s, char delimiter,
            bool[] charEscapeFlags, StringEscapeHandling stringEscapeHandling, JsonTextWriter client, char[] writeBuffer, CancellationToken cancellationToken)
        {
            await task.ConfigureAwait(false);

            if (!string.IsNullOrEmpty(s))
            {
                await WriteEscapedJavaScriptStringWithoutDelimitersAsync(writer, s, charEscapeFlags, stringEscapeHandling, client, writeBuffer, cancellationToken).ConfigureAwait(false);
            }

            await writer.WriteAsync(delimiter).ConfigureAwait(false);
        }

        public static async Task WriteCharAsync(Task task, TextWriter writer, char c, CancellationToken cancellationToken)
        {
            await task.ConfigureAwait(false);
            await writer.WriteAsync(c, cancellationToken).ConfigureAwait(false);
        }

        private static Task WriteEscapedJavaScriptStringWithoutDelimitersAsync(
            TextWriter writer, string s, bool[] charEscapeFlags, StringEscapeHandling stringEscapeHandling,
            JsonTextWriter client, char[] writeBuffer, CancellationToken cancellationToken)
        {
            int i = FirstCharToEscape(s, charEscapeFlags, stringEscapeHandling);
            return i == -1
                ? writer.WriteAsync(s, cancellationToken)
                : WriteDefinitelyEscapedJavaScriptStringWithoutDelimitersAsync(writer, s, i, charEscapeFlags, stringEscapeHandling, client, writeBuffer, cancellationToken);
        }

        private static async Task WriteDefinitelyEscapedJavaScriptStringWithoutDelimitersAsync(
            TextWriter writer, string s, int lastWritePosition, bool[] charEscapeFlags,
            StringEscapeHandling stringEscapeHandling, JsonTextWriter client, char[] writeBuffer,
            CancellationToken cancellationToken)
        {
            if (writeBuffer == null || writeBuffer.Length < lastWritePosition)
            {
                writeBuffer = client.EnsureWriteBuffer(lastWritePosition, UnicodeTextLength);
            }

            if (lastWritePosition != 0)
            {
                s.CopyTo(0, writeBuffer, 0, lastWritePosition);

                // write unchanged chars at start of text.
                await writer.WriteAsync(writeBuffer, 0, lastWritePosition, cancellationToken).ConfigureAwait(false);
            }

            int length;
            bool isEscapedUnicodeText = false;
            string escapedValue = null;

            for (int i = lastWritePosition; i < s.Length; i++)
            {
                char c = s[i];

                if (c < charEscapeFlags.Length && !charEscapeFlags[c])
                {
                    continue;
                }

                switch (c)
                {
                    case '\t':
                        escapedValue = @"\t";
                        break;

                    case '\n':
                        escapedValue = @"\n";
                        break;

                    case '\r':
                        escapedValue = @"\r";
                        break;

                    case '\f':
                        escapedValue = @"\f";
                        break;

                    case '\b':
                        escapedValue = @"\b";
                        break;

                    case '\\':
                        escapedValue = @"\\";
                        break;

                    case '\u0085': // Next Line
                        escapedValue = @"\u0085";
                        break;

                    case '\u2028': // Line Separator
                        escapedValue = @"\u2028";
                        break;

                    case '\u2029': // Paragraph Separator
                        escapedValue = @"\u2029";
                        break;

                    default:
                        if (c < charEscapeFlags.Length || stringEscapeHandling == StringEscapeHandling.EscapeNonAscii)
                        {
                            if (c == '\'' && stringEscapeHandling != StringEscapeHandling.EscapeHtml)
                            {
                                escapedValue = @"\'";
                            }
                            else if (c == '"' && stringEscapeHandling != StringEscapeHandling.EscapeHtml)
                            {
                                escapedValue = @"\""";
                            }
                            else
                            {
                                if (writeBuffer.Length < UnicodeTextLength)
                                {
                                    writeBuffer = client.EnsureWriteBuffer(UnicodeTextLength, 0);
                                }

                                StringUtils.ToCharAsUnicode(c, writeBuffer);

                                isEscapedUnicodeText = true;
                            }
                        }
                        else
                        {
                            continue;
                        }
                        break;
                }

                if (i > lastWritePosition)
                {
                    length = i - lastWritePosition + (isEscapedUnicodeText ? UnicodeTextLength : 0);
                    int start = isEscapedUnicodeText ? UnicodeTextLength : 0;

                    if (writeBuffer.Length < length)
                    {
                        writeBuffer = client.EnsureWriteBuffer(length, UnicodeTextLength);
                    }

                    s.CopyTo(lastWritePosition, writeBuffer, start, length - start);

                    // write unchanged chars before writing escaped text
                    await writer.WriteAsync(writeBuffer, start, length - start, cancellationToken).ConfigureAwait(false);
                }

                lastWritePosition = i + 1;
                if (!isEscapedUnicodeText)
                {
                    await writer.WriteAsync(escapedValue, cancellationToken).ConfigureAwait(false);
                }
                else
                {
                    await writer.WriteAsync(writeBuffer, 0, UnicodeTextLength, cancellationToken).ConfigureAwait(false);
                    isEscapedUnicodeText = false;
                }
            }

            length = s.Length - lastWritePosition;

            if (length != 0)
            {
                if (writeBuffer.Length < length)
                {
                    writeBuffer = client.EnsureWriteBuffer(length, 0);
                }

                s.CopyTo(lastWritePosition, writeBuffer, 0, length);

                // write remaining text
                await writer.WriteAsync(writeBuffer, 0, length, cancellationToken).ConfigureAwait(false);
            }
        }
#endif
    }
}