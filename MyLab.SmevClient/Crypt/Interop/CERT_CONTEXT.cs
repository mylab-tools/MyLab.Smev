using System;
using System.Runtime.InteropServices;

namespace MyLab.SmevClient.Crypt
{
    internal static partial class Interop
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct CERT_CONTEXT
        {
            public uint dwCertEncodingType;
            public IntPtr pbCertEncoded;
            public uint cbCertEncoded;
            public IntPtr pCertInfo;
            public IntPtr hCertStore;
        }
    }
}
