using System;
using System.Text;
using System.Runtime.InteropServices;

namespace MyLab.SmevClient.Crypt
{
    internal static partial class Interop
    {
        [DllImport(Libraries.Kernel32)]
        public static extern int GetLastError();

        [DllImport(Libraries.Kernel32, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern
        int FormatMessage(
            [In] uint dwFlags,
            [In] IntPtr lpSource,
            [In] int dwMessageId,
            [In] int dwLanguageId,
            [Out] StringBuilder lpBuffer,
            [In] int nSize,
            [In] IntPtr vaListArguments);
    }

}
