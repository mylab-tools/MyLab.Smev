using System.Security.Cryptography.Xml;
using System;

namespace MyLab.SmevClient.Crypt
{
    internal static partial class Interop
    {
#if Windows_NT
        internal static partial class Libraries
        {
            internal const string Crypt32 = "Crypt32.dll";
            internal const string Advapi32 = "Advapi32.dll";
            internal const string Kernel32 = "kernel32.dll";
        }
#else
        internal static partial class Libraries
        {
            internal const string Crypt32 = "libcapi20";
            internal const string Advapi32 = "libcapi20";
            internal const string Kernel32 = "librdrsup";
        }
#endif

    }
}
