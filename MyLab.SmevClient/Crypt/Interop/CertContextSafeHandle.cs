using Microsoft.VisualBasic;
using Microsoft.Win32.SafeHandles;

namespace MyLab.SmevClient.Crypt
{
    public class CertContextSafeHandle: SafeHandleZeroOrMinusOneIsInvalid
    {
        protected CertContextSafeHandle()
            : base(true)
        {
        }

        protected override bool ReleaseHandle()
        {
            if (IsInvalid || IsClosed)
            {
                return true;
            }

            return Interop.CertFreeCertificateContext(handle);
        }
    }
}
