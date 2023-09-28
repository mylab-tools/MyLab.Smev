using System;
using System.Security.Cryptography;

namespace MyLab.SmevClient.Crypt
{
    public class GostR3411_2012_256HashAlgorithm : HashAlgorithm
    {
        private CspSafeHandle _cspHandle;
        private HashSafeHandle _hashHandle;

        public GostR3411_2012_256HashAlgorithm()
        {
            HashSizeValue = 256;
            
            if (!Interop.CryptAcquireContext(
               out _cspHandle, null, Interop.Consts.CP_GR3410_2012_PROV,
               Interop.Consts.PROV_GOST_2012_256, Interop.Consts.CRYPT_VERIFYCONTEXT))
            {
                throw new Interop.CPLastErrorException();
            }

            Initialize();
        }

        ~GostR3411_2012_256HashAlgorithm()
        {
            Dispose(false);
        }

        public override int InputBlockSize => 64;

        public override int OutputBlockSize => 64;

        public override void Initialize()
        {
            _hashHandle?.Close();
            _hashHandle = null;            

            if (!Interop.CryptCreateHash(
                _cspHandle, Interop.Consts.CALG_GR3411_2012_256, IntPtr.Zero,
                0, out _hashHandle))
            {
                throw new Interop.CPLastErrorException();
            }
        }

        protected unsafe override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            if(array.Length == 0 || cbSize == 0)
            {
                return;
            }

            fixed(byte* pbData = &array[ibStart])
            {
                if (!Interop.CryptHashData(_hashHandle, new IntPtr(pbData), cbSize, 0))
                {
                    throw new Interop.CPLastErrorException();
                }
            }            
        }

        protected unsafe override byte[] HashFinal()
        {
            int dataLength = 32;
            var data = new byte[dataLength];

            fixed(void* ptr = data)
            {                
                if (!Interop.CryptGetHashParam(
                    _hashHandle, Interop.Consts.HP_HASHVAL, new IntPtr(ptr), ref dataLength, 0))
                {
                    throw new Interop.CPLastErrorException();
                }
            }

            if(dataLength != data.Length)
            {
                throw new Exception("Неверный размер хэша!");
            }

            return data;
        }

        protected override void Dispose(bool disposing)
        {
            _hashHandle?.Close();
            _hashHandle = null;

            _cspHandle?.Close();
            _cspHandle = null;

            base.Dispose(disposing);
        }
    }
}
