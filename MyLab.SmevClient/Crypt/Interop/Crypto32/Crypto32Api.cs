using System;
using System.Text;
using System.Runtime.InteropServices;
using static MyLab.SmevClient.Crypt.Interop;

namespace MyLab.SmevClient.Crypt
{
    internal static partial class Interop
    {
        public static CertStoreSafeHandle PFXImportCertStore(
            [In] ref CRYPT_DATA_BLOB pPfx,
            [In] IntPtr szPassword,
            [In] uint dwFlags)
        {
            if (IsWindows)
            {
                return InteropWindows.PFXImportCertStore(ref pPfx, szPassword, dwFlags);
            }
            return InteropLinux.PFXImportCertStore(ref pPfx, szPassword, dwFlags);

        }

        public static CertStoreSafeHandle CertOpenStore(CertStoreProvider lpszStoreProvider, CertEncodingType dwMsgAndCertEncodingType, IntPtr hCryptProv, CertStoreFlags dwFlags, string pvPara)
        {
            if (IsWindows)
            {
                return InteropWindows.CertOpenStore(lpszStoreProvider, dwMsgAndCertEncodingType, hCryptProv, dwFlags, pvPara);
            }
            return InteropLinux.CertOpenStore(lpszStoreProvider, dwMsgAndCertEncodingType, hCryptProv, dwFlags, pvPara);

        }


        public static bool CertControlStore(CertStoreSafeHandle hCertStore, CertControlStoreFlags dwFlags, CertControlStoreType dwControlType, IntPtr pvCtrlPara)
        {
            if (IsWindows)
            {
                return InteropWindows.CertControlStore(hCertStore, dwFlags, dwControlType, pvCtrlPara);
            }
            return InteropLinux.CertControlStore(hCertStore, dwFlags, dwControlType, pvCtrlPara);

        }


        public static CertContextSafeHandle CertFindCertificateInStore(
            [In] CertStoreSafeHandle hCertStore,
            [In] uint dwCertEncodingType,
            [In] uint dwFindFlags,
            [In] uint dwFindType,
            [In] IntPtr pvFindPara,
            [In] IntPtr pPrevCertContext)
        {
            if (IsWindows)
            {
                return InteropWindows.CertFindCertificateInStore(hCertStore, dwCertEncodingType, dwFindFlags, dwFindType, pvFindPara, pPrevCertContext);
            }
            return InteropLinux.CertFindCertificateInStore(hCertStore, dwCertEncodingType, dwFindFlags, dwFindType, pvFindPara, pPrevCertContext);
        }

        public static  bool CertCloseStore(
            [In] IntPtr hCertStore,
            [In] uint dwFlags)
        {
            if (IsWindows)
            {
                return InteropWindows.CertCloseStore(hCertStore, dwFlags);
            }
            return InteropLinux.CertCloseStore(hCertStore, dwFlags);

        }

        public static bool CertFreeCertificateContext([In] IntPtr pCertContext)
        {
            if (IsWindows)
            {
                return InteropWindows.CertFreeCertificateContext(pCertContext);
            }
            return InteropLinux.CertFreeCertificateContext(pCertContext);
        }

        public static  bool CryptAcquireCertificatePrivateKey(
            [In] CertContextSafeHandle pCert,
            [In] uint dwFlags,
            [In] IntPtr pvReserved,
            [Out] out CspSafeHandle phCryptProv,
            [In, Out] ref uint pdwKeySpec,
            [In, Out] ref bool pfCallerFreeProv)
        {
            if (IsWindows)
            {
                return InteropWindows.CryptAcquireCertificatePrivateKey(pCert, dwFlags, pvReserved,out phCryptProv, ref pdwKeySpec, ref pfCallerFreeProv);
            }
            return InteropLinux.CryptAcquireCertificatePrivateKey(pCert, dwFlags, pvReserved, out phCryptProv, ref pdwKeySpec, ref pfCallerFreeProv);
        }

    }
}
