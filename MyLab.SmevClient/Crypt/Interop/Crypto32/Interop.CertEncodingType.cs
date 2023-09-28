namespace MyLab.SmevClient.Crypt
{
    internal static partial class Interop
    {
        internal enum CertEncodingType : int
        {
            PKCS_7_ASN_ENCODING = 0x10000,
            X509_ASN_ENCODING = 0x00001,

            All = PKCS_7_ASN_ENCODING | X509_ASN_ENCODING,
        }

    }
}
