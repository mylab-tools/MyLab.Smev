using System;
using System.Text;
using System.Runtime.InteropServices;

namespace MyLab.SmevClient.Crypt
{
    internal static partial class InteropWindows
    {
        [DllImport(Libraries.Advapi32, CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "CryptAcquireContextA")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CryptAcquireContext(
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


        [DllImport(Libraries.Crypt32, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal unsafe static extern bool CryptSignMessage(
                [In] ref CRYPT_SIGN_MESSAGE_PARA pSignPara,
                // Значение TRUE , если это отсоединяемая сигнатура.
                // В противном случае — FALSE. Если для этого параметра задано значение TRUE, в pbSignedBlob кодируется только хэш со знаком.
                // В противном случае кодируются как rgpbToBeSigned , так и хэш со знаком.
                [In, MarshalAs(UnmanagedType.Bool)] bool fDetachedSignature,
                //Количество элементов массива в rgpbToBeSigned и rgcbToBeSigned.
                //Этот параметр должен иметь значение один, если только fDetachedSignature не имеет значение TRUE.
                [In] uint cToBeSigned,
                // Массив указателей на буферы, содержащие содержимое для подписи.
                [In] IntPtr rgpbToBeSigned,

                // Массив размеров (в байтах) буферов содержимого, на которые указывает rgpbToBeSigned.
                [In] IntPtr rgcbToBeSigned,

                // Указатель на буфер для получения закодированного хэша со знаком, если fDetachedSignature имеет значение TRUE,
                // или на закодированное содержимое и хэш со знаком, если fDetachedSignature имеет значение FALSE
                [Out] IntPtr signature,

                // Указатель на DWORD , указывающий размер буфера pbSignedBlob (в байтах).
                // При возврате функции эта переменная содержит размер подписанного и закодированного сообщения в байтах
                [In, Out] ref uint signatureLength);
    }
}
