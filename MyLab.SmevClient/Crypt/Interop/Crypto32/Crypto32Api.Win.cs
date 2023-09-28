using System;
using System.Text;
using System.Runtime.InteropServices;
using static MyLab.SmevClient.Crypt.Interop;

namespace MyLab.SmevClient.Crypt
{
    internal static partial class InteropWindows
    {
        [DllImport(Libraries.Crypt32, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern 
            CertStoreSafeHandle PFXImportCertStore(
            [In] ref CRYPT_DATA_BLOB pPfx,
            [In] IntPtr szPassword,
            [In] uint dwFlags);

        public static CertStoreSafeHandle CertOpenStore(CertStoreProvider lpszStoreProvider, CertEncodingType dwMsgAndCertEncodingType, IntPtr hCryptProv, CertStoreFlags dwFlags, string pvPara)
        {
            return CertOpenStore((IntPtr)lpszStoreProvider, dwMsgAndCertEncodingType, hCryptProv, dwFlags, pvPara);
        }

        [DllImport(Libraries.Crypt32, CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "CertOpenStore")]
        private static extern CertStoreSafeHandle CertOpenStore(IntPtr lpszStoreProvider, CertEncodingType dwMsgAndCertEncodingType, IntPtr hCryptProv, CertStoreFlags dwFlags, string pvPara);


        [DllImport(Libraries.Crypt32, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool CertControlStore(CertStoreSafeHandle hCertStore, CertControlStoreFlags dwFlags, CertControlStoreType dwControlType, IntPtr pvCtrlPara);


        [DllImport(Libraries.Crypt32, SetLastError = true)]
        public static extern
            CertContextSafeHandle CertFindCertificateInStore(
            [In] CertStoreSafeHandle hCertStore,
            [In] uint dwCertEncodingType,
            [In] uint dwFindFlags,
            [In] uint dwFindType,
            [In] IntPtr pvFindPara,
            [In] IntPtr pPrevCertContext);

        [DllImport(Libraries.Crypt32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern
        bool CertCloseStore(
            [In] IntPtr hCertStore,
            [In] uint dwFlags);

        [DllImport(Libraries.Crypt32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern 
        bool CertFreeCertificateContext(
            [In] IntPtr pCertContext);

        [DllImport(Libraries.Crypt32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern
        bool CryptAcquireCertificatePrivateKey(
            [In] CertContextSafeHandle pCert,
            [In] uint dwFlags,
            [In] IntPtr pvReserved,
            [Out] out CspSafeHandle phCryptProv,
            [In, Out] ref uint pdwKeySpec,
            [In, Out] ref bool pfCallerFreeProv);

    }
}
