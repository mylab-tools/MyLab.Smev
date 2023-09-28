using System;
using System.Text;
using System.Runtime.InteropServices;
using static MyLab.SmevClient.Crypt.Interop;

namespace MyLab.SmevClient.Crypt
{
    internal static partial class Interop
    {
        public static int GetLastError()
        {
            if (IsWindows)
            {
                return InteropWindows.GetLastError();
            }
            return InteropLinux.GetLastError();
        }

        public static  int FormatMessage(
            [In] uint dwFlags,
            [In] IntPtr lpSource,
            [In] int dwMessageId,
            [In] int dwLanguageId,
            [Out] StringBuilder lpBuffer,
            [In] int nSize,
            [In] IntPtr vaListArguments)
        {
            if (IsWindows)
            {
                return InteropWindows.FormatMessage(dwFlags, lpSource, dwMessageId, dwLanguageId, lpBuffer,nSize, vaListArguments);
            }
            return InteropLinux.FormatMessage(dwFlags, lpSource, dwMessageId, dwLanguageId, lpBuffer, nSize, vaListArguments);
        }
    }

}
