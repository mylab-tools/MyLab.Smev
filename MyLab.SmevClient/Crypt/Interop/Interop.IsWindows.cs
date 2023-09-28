using System.Security.Cryptography.Xml;
using System;
using System.Runtime.InteropServices;

namespace MyLab.SmevClient.Crypt
{
    internal partial class Interop
    {
        internal static bool IsWindows = System.Runtime.InteropServices.RuntimeInformation
                                                   .IsOSPlatform(OSPlatform.Windows);
    }
}
