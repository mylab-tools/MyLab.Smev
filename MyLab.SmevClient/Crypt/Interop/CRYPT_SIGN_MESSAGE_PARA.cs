using System;
using System.Runtime.InteropServices;

namespace MyLab.SmevClient.Crypt
{
    [Flags]
    internal enum MsgEncodingType : int
    {
        PKCS_7_ASN_ENCODING = 0x10000,
        X509_ASN_ENCODING = 0x00001,

        All = PKCS_7_ASN_ENCODING | X509_ASN_ENCODING,
    }


    [Flags]
    public enum CryptMsgActionFlags : int
    {
        /// <summary>
        /// If the encoded output is to be a CMSG_SIGNED inner content of an outer cryptographic message such as a CMSG_ENVELOPED
        /// message, the CRYPT_MESSAGE_BARE_CONTENT_OUT_FLAG must be set.
        /// </summary>
        CRYPT_MESSAGE_BARE_CONTENT_OUT_FLAG = 0x00000001,

        /// <summary>CRYPT_MESSAGE_ENCAPSULATED_CONTENT_OUT_FLAG can be set to encapsulate non-data inner content into an OCTET STRING.</summary>
        CRYPT_MESSAGE_ENCAPSULATED_CONTENT_OUT_FLAG = 0x00000002,

        /// <summary>
        /// CRYPT_MESSAGE_KEYID_SIGNER_FLAG can be set to identify signers by their Key Identifier and not their Issuer and Serial Number.
        /// </summary>
        CRYPT_MESSAGE_KEYID_SIGNER_FLAG = 0x00000004,

        /// <summary>
        /// CRYPT_MESSAGE_SILENT_KEYSET_FLAG can be set to suppress any UI by the CSP. For more information about the CRYPT_SILENT flag,
        /// see CryptAcquireContext.
        /// </summary>
        CRYPT_MESSAGE_SILENT_KEYSET_FLAG = 0x00000040,
    }


    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct CRYPTOAPI_BLOB
    {
        public CRYPTOAPI_BLOB(int cbData, byte* pbData)
        {
            this.cbData = cbData;
            this.pbData = pbData;
        }

        public int cbData;
        public byte* pbData;

        public byte[] ToByteArray()
        {
            if (cbData == 0)
            {
                return Array.Empty<byte>();
            }

            byte[] array = new byte[cbData];
            Marshal.Copy((IntPtr)pbData, array, 0, cbData);
            return array;
        }
    }


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct CRYPT_ALGORITHM_IDENTIFIER
    {
        public IntPtr pszObjId;
        public CRYPTOAPI_BLOB Parameters;
    }


    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CRYPT_SIGN_MESSAGE_PARA
    {
        /// <summary>Size of this structure in bytes.</summary>
        public int dwSize;
        public MsgEncodingType dwMsgEncodingType;

        /// <summary>
        /// <para>A pointer to the CERT_CONTEXT to be used in the signing.</para>
        /// <para>
        /// Either the CERT_KEY_PROV_INFO_PROP_ID, or CERT_KEY_CONTEXT_PROP_ID property must be set for the context to provide access to
        /// the private signature key.
        /// </para>
        /// </summary>
        public IntPtr pSigningCert;

        /// <summary>CRYPT_ALGORITHM_IDENTIFIER containing the hashing algorithm used to hash the data to be signed.</summary>
        public CRYPT_ALGORITHM_IDENTIFIER hashAlgorithm;

        /// <summary>Not currently used, and must be set to <c>NULL</c>.</summary>
        public IntPtr pvHashAuxInfo;

        /// <summary>
        /// Number of elements in the <c>rgpMsgCert</c> array of CERT_CONTEXT structures. If set to zero no certificates are included in
        /// the signed message.
        /// </summary>
        public uint cMsgCert;

        /// <summary>
        /// Array of pointers to CERT_CONTEXT structures to be included in the signed message. If the <c>pSigningCert</c> is to be
        /// included, a pointer to it must be in the <c>rgpMsgCert</c> array.
        /// </summary>
        public IntPtr rgpMsgCert;

        /// <summary>
        /// Number of elements in the <c>rgpMsgCrl</c> array of pointers to CRL_CONTEXT structures. If set to zero, no
        /// <c>CRL_CONTEXT</c> structures are included in the signed message.
        /// </summary>
        public uint cMsgCrl;

        /// <summary>Array of pointers to CRL_CONTEXT structures to be included in the signed message.</summary>
        public IntPtr rgpMsgCrl;

        /// <summary>
        /// Number of elements in the <c>rgAuthAttr</c> array. If no authenticated attributes are present in <c>rgAuthAttr</c>, this
        /// member is set to zero.
        /// </summary>
        public uint cAuthAttr;

        /// <summary>
        /// Array of pointers to CRYPT_ATTRIBUTE structures, each holding authenticated attribute information. If there are
        /// authenticated attributes present, the PKCS #9 standard dictates that there must be at least two attributes present, the
        /// content type object identifier (OID), and the hash of the message itself. These attributes are automatically added by the system.
        /// </summary>
        public IntPtr rgAuthAttr;

        /// <summary>
        /// Number of elements in the <c>rgUnauthAttr</c> array. If no unauthenticated attributes are present in <c>rgUnauthAttr</c>,
        /// this member is zero.
        /// </summary>
        public uint cUnauthAttr;

        /// <summary>
        /// Array of pointers to CRYPT_ATTRIBUTE structures each holding an unauthenticated attribute information. Unauthenticated
        /// attributes can be used to contain countersignatures, among other uses.
        /// </summary>
        public IntPtr rgUnauthAttr;

        /// <summary>
        /// <para>
        /// Normally zero. If the encoded output is to be a CMSG_SIGNED inner content of an outer cryptographic message such as a
        /// CMSG_ENVELOPED message, the CRYPT_MESSAGE_BARE_CONTENT_OUT_FLAG must be set. If it is not set, the message will be encoded
        /// as an inner content type of CMSG_DATA.
        /// </para>
        /// <para>
        /// CRYPT_MESSAGE_ENCAPSULATED_CONTENT_OUT_FLAG can be set to encapsulate non-data inner content into an OCTET STRING.
        /// CRYPT_MESSAGE_KEYID_SIGNER_FLAG can be set to identify signers by their Key Identifier and not their Issuer and Serial Number.
        /// </para>
        /// <para>
        /// CRYPT_MESSAGE_SILENT_KEYSET_FLAG can be set to suppress any UI by the CSP. For more information about the CRYPT_SILENT flag,
        /// see CryptAcquireContext.
        /// </para>
        /// </summary>
        public CryptMsgActionFlags dwFlags;

        /// <summary>
        /// Normally zero. Set to the encoding type of the input message if that input to be signed is the encoded output of another
        /// cryptographic message.
        /// </summary>
        public uint dwInnerContentType;


    }
}
