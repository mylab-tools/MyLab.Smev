using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace MyLab.SmevClient.Crypt
{
    public class GostAsymmetricAlgorithm : AsymmetricAlgorithm
    {
        private const byte SIGN_BUFF_SIZE = 64;

        private readonly uint _keySpec;

        private CspSafeHandle _cspHandle;
        private CertContextSafeHandleHandler _certHandle = null;

        private readonly Lazy<byte[]> _certRawData;

        static GostAsymmetricAlgorithm()
        {
            CryptoConfig.AddAlgorithm(typeof(GostAsymmetricAlgorithm), "Smev3Signature");
            CryptoConfig.AddAlgorithm(typeof(GostSignatureDescription), XmlDsigConsts.XmlDsigGost3410_2012_256Url);
            CryptoConfig.AddAlgorithm(typeof(GostR3411_2012_256HashAlgorithm), XmlDsigConsts.XmlDsigGost3411_2012_256Url);
        }

        protected GostAsymmetricAlgorithm()
        {
            _certRawData = new Lazy<byte[]>(() => GetCertRawData(), true);
        }

        public override string SignatureAlgorithm => XmlDsigConsts.XmlDsigGost3410_2012_256Url;

        public byte[] CertRawData => _certRawData.Value;

        public unsafe GostAsymmetricAlgorithm(CertHandleProvider certHandleProvider)
         : this()
        {

            try
            {
                _certHandle = certHandleProvider.Provide();

                bool callerFreeProvider = false;
                if (!Interop.CryptAcquireCertificatePrivateKey(
                    _certHandle.CertHandle, Interop.Consts.CRYPT_ACQUIRE_USE_PROV_INFO_FLAG,
                    IntPtr.Zero, out _cspHandle, ref _keySpec, ref callerFreeProvider))
                {
                    throw new Interop.LastErrorException();
                }
            }
            catch
            {
                Dispose(true);
                throw;
            }
        }

        /// <summary>
        /// Подпись хэш
        /// </summary>
        /// <param name="hashData"></param>
        /// <returns></returns>
        public unsafe byte[] CreateHashSignature(byte[] hashData)
        {
            if (hashData == null || hashData.Length == 0)
            {
                throw new ArgumentException($"Параметр {nameof(hashData)} должен быть не пустым массивом.");
            }

            HashSafeHandle hashHandle = null;
            try
            {
                if (!Interop.CryptCreateHash(
                    _cspHandle, Interop.Consts.CALG_GR3411_2012_256, IntPtr.Zero,
                    0, out hashHandle))
                {
                    throw new Interop.LastErrorException();
                }

                fixed (void* ptrHashData = hashData)
                {
                    if (!Interop.CryptSetHashParam(hashHandle, Interop.Consts.HP_HASHVAL, new IntPtr(ptrHashData), 0))
                    {
                        throw new Interop.LastErrorException();
                    }

                    var signData = new byte[SIGN_BUFF_SIZE];
                    int signDataLen = signData.Length;

                    fixed (byte* ptrSignData = signData)
                    {
                        if (!Interop.CryptSignHash(hashHandle, _keySpec, IntPtr.Zero, 0, new IntPtr(ptrSignData), ref signDataLen))
                        {
                            throw new Interop.LastErrorException();
                        }
                    }

                    Array.Reverse(signData);

                    return signData;
                }
            }
            finally
            {
                hashHandle?.Close();
            }
        }

        protected override void Dispose(bool disposing)
        {
            _certHandle?.Dispose();
            _cspHandle?.Close();

            _certHandle = null;
            _cspHandle = null;

            base.Dispose(disposing);
        }

        #region private

        private unsafe byte[] GetCertRawData()
        {
            if (_certHandle == null || _certHandle.CertHandle.IsInvalid)
            {
                throw new Exception("Объект не инициалирован.");
            }

            var certContext = Marshal.PtrToStructure<Interop.CERT_CONTEXT>(
                                                    _certHandle.CertHandle.DangerousGetHandle());

            var certEncoded = new byte[certContext.cbCertEncoded];

            fixed (void* ptr = certEncoded)
            {
                Buffer.MemoryCopy(certContext.pbCertEncoded.ToPointer(), ptr,
                    certEncoded.Length, certContext.cbCertEncoded);
            }

            return certEncoded;
        }



        #endregion
    }

    internal static class HexConvert
    {
        internal static byte[] HexToArray(string srcHexString)
        {
            string hexString = srcHexString.Replace(" ", "");
            return Enumerable.Range(0, hexString.Length)
                            .Where(x => x % 2 == 0)
                            .Select(x => Convert.ToByte(hexString.Substring(x, 2), 16))
                            .ToArray();
        }

    }
}