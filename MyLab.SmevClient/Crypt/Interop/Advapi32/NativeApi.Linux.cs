using System;
using System.Text;
using System.Runtime.InteropServices;

namespace MyLab.SmevClient.Crypt
{
    internal static partial class InteropLinux
    {
        [DllImport(Libraries.Advapi32, CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "CryptAcquireContextA")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern  bool CryptAcquireContext(
            [Out] out CspSafeHandle phProv,
            [In] string pszContainer,
            [In] string pszProvider,
            [In] uint dwProvType,
            [In] uint dwFlags);

        [DllImport(Libraries.Advapi32, CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern 
        bool CryptReleaseContext(
            [In] IntPtr hProv,
            [In] uint dwFlags);

        [DllImport(Libraries.Advapi32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern
        bool CryptCreateHash(
            [In] CspSafeHandle hProv,
            [In] uint algid,
            [In] IntPtr hKey,
            [In] int dwFlags,
            [Out] out HashSafeHandle phHash);

        [DllImport(Libraries.Advapi32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern
        bool CryptDestroyHash(
          [In] IntPtr hHash
        );


        [DllImport(Libraries.Advapi32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern
        bool CryptHashData(
            [In] HashSafeHandle hHash,
            [In] IntPtr pbData,
            [In] int dataLen,
            [In] int flags);

        [DllImport(Libraries.Advapi32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern 
        bool CryptGetHashParam(
            [In] HashSafeHandle hHash,
            [In] uint dwParam,
            [In, Out] IntPtr pbData,
            [In, Out] ref int pdwDataLen,
            [In] int dwFlags);

        [DllImport(Libraries.Advapi32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern
        bool CryptSetHashParam(
            [In] HashSafeHandle hHash,
            [In] uint dwParam,
            [In] IntPtr pbData,
            [In] int dwFlags);

        [DllImport(Libraries.Advapi32, SetLastError = true, CharSet = CharSet.Ansi, EntryPoint = "CryptSignHashA")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern 
        bool CryptSignHash(
            [In] HashSafeHandle hHash,
            [In] uint keySpec,
            [In] IntPtr description,
            [In] uint flags,
            [Out] IntPtr signature,
            [In, Out] ref int signatureLen);
    }
}
