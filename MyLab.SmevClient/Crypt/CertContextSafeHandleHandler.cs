using System;

namespace MyLab.SmevClient.Crypt
{
    internal class CertContextSafeHandleHandler : IDisposable
    {
        private CertStoreSafeHandle _storeHandle;
        private CertContextSafeHandle _certHandle;

        internal CertContextSafeHandle CertHandle => _certHandle;
        internal CertContextSafeHandleHandler(CertStoreSafeHandle storeHandle, CertContextSafeHandle certHandle)
        {
            _storeHandle = storeHandle;
            _certHandle = certHandle;
        }
        public void Dispose()
        {
            _certHandle?.Close();
            _storeHandle?.Close();

            _certHandle = null;
            _storeHandle = null;
        }
    }
}