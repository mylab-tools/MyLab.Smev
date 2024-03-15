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

        public static bool CryptGetHashParam(
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

        public static bool CryptSignHash(
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pSignPara"></param>
        /// <param name="fDetachedSignature">
        ///    Значение TRUE , если это отсоединяемая сигнатура.
        ///     В противном случае — FALSE.Если для этого параметра задано значение TRUE, в pbSignedBlob кодируется только хэш со знаком.
        ///     В противном случае кодируются как rgpbToBeSigned, так и хэш со знаком.
        ///    </param>
        /// <param name="rgpbToBeSigned">Массив на данные которые подписываем.</param>
        /// <param name="signature">Подпись</param>
        /// <returns></returns>
        internal unsafe static bool CryptSignMessage(
                       [In] ref CRYPT_SIGN_MESSAGE_PARA pSignPara,
                       [In, MarshalAs(UnmanagedType.Bool)] bool fDetachedSignature,
                       [In] byte[] rgpbToBeSigned,
                       [Out] out byte[] signature
                       )
        {
            unsafe
            {
                // размер данных для подписи
                uint* sizesData = stackalloc uint[] { (uint)rgpbToBeSigned.LongLength };

                // фиксация данных для подписи
                fixed (byte* ptrData = &rgpbToBeSigned[0])
                {
                    // массив с исходными данными
                    IntPtr* rgpbToBeSignedPtr = stackalloc IntPtr[] { new IntPtr(ptrData) };

                    uint signatureLength = 0;
                   
                    // резервирование данных
                    if (!CryptSignMessage(ref pSignPara, fDetachedSignature, 1, new IntPtr(rgpbToBeSignedPtr), new IntPtr(sizesData), IntPtr.Zero, ref signatureLength))
                    {
                        signature = null;
                        return false;
                    }

                    var signatureBuffer = Marshal.AllocHGlobal((int)signatureLength);
                    try
                    {
                        // создание подписи
                        if (!CryptSignMessage(ref pSignPara, fDetachedSignature, 1, new IntPtr(rgpbToBeSignedPtr), new IntPtr(sizesData), signatureBuffer, ref signatureLength))
                        {
                            signature = null;
                            return false;
                        }
                        signature = new byte[signatureLength];
                        Marshal.Copy(signatureBuffer, signature, 0, (int)signatureLength);
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(signatureBuffer);
                    }


                }
            }

            return true;
        }

        private unsafe static bool CryptSignMessage(
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
                [In, Out] ref uint signatureLength)
        {
            if (IsWindows)
            {

                return InteropWindows.CryptSignMessage(ref pSignPara, fDetachedSignature, cToBeSigned, rgpbToBeSigned, rgcbToBeSigned, signature, ref signatureLength);
            }
            return InteropLinux.CryptSignMessage(ref pSignPara, fDetachedSignature, cToBeSigned, rgpbToBeSigned, rgcbToBeSigned, signature, ref signatureLength);
        }
    }
}
