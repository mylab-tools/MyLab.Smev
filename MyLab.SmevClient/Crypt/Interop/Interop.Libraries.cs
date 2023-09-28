using System.Security.Cryptography.Xml;
using System;
using System.Runtime.InteropServices;

namespace MyLab.SmevClient.Crypt
{
    internal static partial class InteropWindows
    {
        internal static partial class Libraries
        {
            internal const string Crypt32 = "Crypt32.dll";
            internal const string Advapi32 = "Advapi32.dll";
            internal const string Kernel32 = "kernel32.dll";
        }

    }
    internal static partial class InteropLinux
    {
        internal static partial class Libraries
        {
            internal const string Crypt32 = "libcapi20";
            internal const string Advapi32 = "libcapi20";
            internal const string Kernel32 = "librdrsup";
        }

    }
}
