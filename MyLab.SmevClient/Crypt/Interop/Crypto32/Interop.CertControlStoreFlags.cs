using System;

namespace MyLab.SmevClient.Crypt
{
    internal static partial class Interop
    {
        [Flags]
        internal enum CertControlStoreFlags : int
        {
            None = 0x00000000,
        }

    }
}
