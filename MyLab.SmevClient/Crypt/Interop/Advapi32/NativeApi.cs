using System;
using System.Text;
using System.Runtime.InteropServices;

namespace MyLab.SmevClient.Crypt
{
    internal static partial class Interop
    {
        public static bool CryptAcquireContext(
            [Out] out CspSafeHandle phProv,
            [In] string pszContainer,
            [In] string pszProvider,
            [In] uint dwProvType,
            [In] uint dwFlags)
        {
            if (IsWindows)
            {
                return InteropWindows.CryptAcquireContext(out phProv, pszContainer, pszProvider, dwProvType, dwFlags);
            }
            return InteropLinux.CryptAcquireContext(out phProv, pszContainer, pszProvider, dwProvType, dwFlags);
        }

        public static bool CryptReleaseContext(
             [In] IntPtr hProv,
             [In] uint dwFlags)
        {
            if (IsWindows)
            {
                return InteropWindows.CryptReleaseContext(hProv, dwFlags);
            }
            return InteropLinux.CryptReleaseContext(hProv, dwFlags);

        }

        public static bool CryptCreateHash(
            [In] CspSafeHandle hProv,
            [In] uint algid,
            [In] IntPtr hKey,
            [In] int dwFlags,
            [Out] out HashSafeHandle phHash)
        {
            if (IsWindows)
            {
                return InteropWindows.CryptCreateHash(hProv, algid, hKey, dwFlags, out phHash);
            }
            return InteropLinux.CryptCreateHash(hProv, algid, hKey, dwFlags, out phHash);
        }

        public static bool CryptDestroyHash([In] IntPtr hHash)
        {
            if (IsWindows)
            {
                return InteropWindows.CryptDestroyHash(hHash);
            }
            return InteropLinux.CryptDestroyHash(hHash);
        }


        public static bool CryptHashData(
            [In] HashSafeHandle hHash,
            [In] IntPtr pbData,
            [In] int dataLen,
            [In] int flags)
        {
            if (IsWindows)
            {
                return InteropWindows.CryptHashData(hHash, pbData, dataLen, flags);
            }
            return InteropLinux.CryptHashData(hHash, pbData, dataLen, flags);
        }

        public static  bool CryptGetHashParam(
            [In] HashSafeHandle hHash,
            [In] uint dwParam,
            [In, Out] IntPtr pbData,
            [In, Out] ref int pdwDataLen,
            [In] int dwFlags)
        {
            if (IsWindows)
            {
                return InteropWindows.CryptGetHashParam(hHash, dwParam, pbData, ref pdwDataLen, dwFlags);
            }
            return InteropLinux.CryptGetHashParam(hHash, dwParam, pbData, ref pdwDataLen, dwFlags);
        }

        public static bool CryptSetHashParam(
            [In] HashSafeHandle hHash,
            [In] uint dwParam,
            [In] IntPtr pbData,
            [In] int dwFlags)
        {
            if (IsWindows)
            {
                return InteropWindows.CryptSetHashParam(hHash, dwParam, pbData, dwFlags);
            }
            return InteropLinux.CryptSetHashParam(hHash, dwParam, pbData, dwFlags);
        }

        public static  bool CryptSignHash(
            [In] HashSafeHandle hHash,
            [In] uint keySpec,
            [In] IntPtr description,
            [In] uint flags,
            [Out] IntPtr signature,
            [In, Out] ref int signatureLen)
        {
            if (IsWindows)
            {
                return InteropWindows.CryptSignHash(hHash, keySpec, description, flags, signature, ref signatureLen);
            }
            return InteropLinux.CryptSignHash(hHash, keySpec, description, flags, signature, ref signatureLen);
        }
    }
}
