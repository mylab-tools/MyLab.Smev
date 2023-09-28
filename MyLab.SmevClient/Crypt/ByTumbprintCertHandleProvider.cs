using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
[assembly: InternalsVisibleTo("UnitTests")]

namespace MyLab.SmevClient.Crypt
{
    class ByTumbprintCertHandleProvider : CertHandleProvider
    {
        private readonly StoreLocation _storeLocation;
        private readonly string _thumbPrint;

        internal ByTumbprintCertHandleProvider(StoreLocation storeLocation, string thumbPrint)
        {
            if (string.IsNullOrWhiteSpace(thumbPrint))
            {
                throw new ArgumentException("Отпечаток сертификата не может быть пустой строкой");
            }

            _storeLocation = storeLocation;
            _thumbPrint = thumbPrint;
        }

        unsafe internal override CertContextSafeHandleHandler Provide()
        {
            CertStoreSafeHandle storeHandle = null;
            CertContextSafeHandle certHandle = null;
            try
            {
                Interop.CertStoreFlags flag = Interop.CertStoreFlags.CERT_STORE_READONLY_FLAG;
                if (_storeLocation == StoreLocation.CurrentUser)
                {
                    flag |= Interop.CertStoreFlags.CERT_SYSTEM_STORE_CURRENT_USER;
                }
                else
                {
                    flag |= Interop.CertStoreFlags.CERT_SYSTEM_STORE_LOCAL_MACHINE;
                }
                storeHandle = Interop.CertOpenStore(Interop.CertStoreProvider.CERT_STORE_PROV_SYSTEM_A,
                    Interop.CertEncodingType.All, IntPtr.Zero,
                     flag , "My");
                
                
                if (storeHandle.IsInvalid)
                {
                    throw new Interop.LastErrorException();
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
                        throw new Interop.LastErrorException();
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