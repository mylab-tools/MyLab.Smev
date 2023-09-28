using System;

using Microsoft.Win32.SafeHandles;

namespace MyLab.SmevClient.Crypt
{
    public class CspSafeHandle: SafeHandleZeroOrMinusOneIsInvalid
    {
        protected CspSafeHandle()
            : base(true)
        {
        }

        protected override bool ReleaseHandle()
        {
            if (IsInvalid)
            {
                return true;
            }

            var result = Interop.CryptReleaseContext(handle, 0);
            if (result)
            {
                SetHandleAsInvalid();
            }

            return result;
        }
    }
}
