namespace MyLab.SmevClient.Crypt
{
    internal static partial class Interop
    {
        internal enum CertStoreProvider : int
        {
            CERT_STORE_PROV_MEMORY = 2,
            CERT_STORE_PROV_SYSTEM_A = 9,
            CERT_STORE_PROV_SYSTEM_W = 10,
        }

    }
}
