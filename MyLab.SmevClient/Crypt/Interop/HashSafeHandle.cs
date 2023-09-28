using System;

using Microsoft.Win32.SafeHandles;

namespace MyLab.SmevClient.Crypt
{
    public class HashSafeHandle: SafeHandleZeroOrMinusOneIsInvalid
    {
        protected HashSafeHandle()
        : base(true)
        {
        }        

        protected override bool ReleaseHandle()
        {
            if (IsInvalid)
            {
                return true;
            }

            var result = Interop.CryptDestroyHash(handle);

            handle = IntPtr.Zero;

            return result;
        }
    }
}
