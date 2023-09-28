using System;

using Microsoft.Win32.SafeHandles;

namespace MyLab.SmevClient.Crypt
{
    public class CertStoreSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        protected CertStoreSafeHandle()
            : base(true)
        {
        }

        protected override bool ReleaseHandle()
        {
            if (IsInvalid)
            {
                return true;
            }

            var result = Interop.CertCloseStore(handle, 0);

            handle = IntPtr.Zero;

            return result;
        }
    }
}
