using System;
using System.Runtime.InteropServices;

namespace MyLab.SmevClient.Crypt
{
    // Создание подписи в формате Pkcs7, учитывая требования к подписи
    internal class Pkcs7SignatureProcessor
    {
        const string szOID_CP_GOST_R3411_12_256 = "1.2.643.7.1.1.2.2";
        static IntPtr ptrOID_CP_GOST_R3411_12_256;
        private readonly CertContextSafeHandleHandler _certProvider;

        static Pkcs7SignatureProcessor() {

            ptrOID_CP_GOST_R3411_12_256 = Marshal.StringToHGlobalAnsi(szOID_CP_GOST_R3411_12_256);
        }
        public Pkcs7SignatureProcessor(CertHandleProvider cert)
        {
            this._certProvider = cert.Provide();
        }

        /// <summary>
        /// Создание подписи 
        /// </summary>
        /// <param name="data">Данные</param>
        /// <returns>Подпись</returns>
        /// <exception cref="Interop.CPLastErrorException"></exception>
        public byte[] Detached(byte[] data)
        {
            // сертификат подписи
            var certHandle = _certProvider.CertHandle.DangerousGetHandle();

            // массив сертификатов, которые будут добавлены в подпись
            var rgpMsgCert = Marshal.AllocHGlobal(Marshal.SizeOf<IntPtr>());
            Marshal.WriteIntPtr(rgpMsgCert, certHandle);

            try
            {
                var messageParam = new CRYPT_SIGN_MESSAGE_PARA()
                {
                    dwSize = Marshal.SizeOf<CRYPT_SIGN_MESSAGE_PARA>(),
                    dwMsgEncodingType = MsgEncodingType.All,
                    pSigningCert = certHandle,
                    cMsgCert = 1, // В подпись будет добавлен, только сертификат подписанта 
                    rgpMsgCert = rgpMsgCert
                };
                // Алгоритм хеширования фиксирован
                messageParam.hashAlgorithm.pszObjId = ptrOID_CP_GOST_R3411_12_256;
                byte[] signature;
                if (!Interop.CryptSignMessage(ref messageParam, true, data, out signature))
                {
                    throw new Interop.CPLastErrorException();
                }
                return signature;
            }
            finally
            {
                Marshal.FreeHGlobal(rgpMsgCert);
            }
        }


    }
}
