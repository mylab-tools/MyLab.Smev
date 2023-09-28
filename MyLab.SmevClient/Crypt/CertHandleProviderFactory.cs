using MyLab.SmevClient.Options;
using System;

namespace MyLab.SmevClient.Crypt
{
    class CertHandleProviderFactory
    {
        static internal CertHandleProvider Create(ClientCertificateOptions opt)
        {
            if (opt is null)
            {
                throw new ArgumentNullException(nameof(opt));
            }

            if (!string.IsNullOrEmpty(opt.PfxPath))
            {
                return new PfxCertHandleProvider(opt.PfxPath, opt.Password, opt.Thumbprint);
            }
            return new ByTumbprintCertHandleProvider(System.Security.Cryptography.X509Certificates.StoreLocation.CurrentUser, opt.Thumbprint);
        }
    }
}