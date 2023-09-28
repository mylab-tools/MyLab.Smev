using System;
using System.IO;
using System.Text;

namespace MyLab.SmevClient.Crypt
{
    class PfxCertHandleProvider : CertHandleProvider
    {
        private readonly string _pfxPath;
        private readonly string _pfxPassword;
        private readonly string _thumbPrint;

        internal PfxCertHandleProvider(string pfxPath, string pfxPassword, string thumbPrint)
        {
            if (string.IsNullOrWhiteSpace(thumbPrint))
            {
                throw new ArgumentException("Отпечаток сертификата не может быть пустой строкой");
            }

            _pfxPath = pfxPath;
            _pfxPassword = pfxPassword;
            _thumbPrint = thumbPrint;
        }

        unsafe internal override CertContextSafeHandleHandler Provide()
        {
            var pfxData = File.ReadAllBytes(_pfxPath);
            CertStoreSafeHandle storeHandle = null;
            CertContextSafeHandle certHandle = null;
            try
            {
                fixed (byte* ptr = pfxData)
                {
                    var pfxDataBlob = new CRYPT_DATA_BLOB
                    {
                        cbData = pfxData.Length,
                        pbData = new IntPtr(ptr)
                    };

                    var passwordBytes = Encoding.UTF32.GetBytes(_pfxPassword ?? string.Empty);
                    fixed (byte* ptrPassword = passwordBytes)
                    {
                        storeHandle = Interop.PFXImportCertStore(ref pfxDataBlob, new IntPtr(ptrPassword),
                            Interop.Consts.CRYPT_USER_KEYSET | Interop.Consts.PKCS12_IMPORT_SILENT);
                        if (storeHandle.IsInvalid)
                        {
                            throw new Interop.CPLastErrorException();
                        }
                    }
                }

                var thumbPrintData = HexConvert.HexToArray(_thumbPrint);

                fixed (byte* ptr = thumbPrintData)
                {
                    var thumbPrintDataBlob = new CRYPT_DATA_BLOB
                    {
                        cbData = thumbPrintData.Length,
                        pbData = new IntPtr(ptr)
                    };

                    certHandle = Interop.CertFindCertificateInStore(
                        storeHandle, Interop.Consts.PKCS_7_OR_X509_ASN_ENCODING, 0,
                        Interop.Consts.CERT_FIND_SHA1_HASH, new IntPtr(&thumbPrintDataBlob), IntPtr.Zero);
                    if (certHandle.IsInvalid)
                    {
                        throw new Interop.CPLastErrorException();
                    }
                }
                return new CertContextSafeHandleHandler(storeHandle, certHandle);

            }
            catch
            {
                certHandle?.Dispose();
                storeHandle?.Dispose();
                throw;
            }

        }
    }
}