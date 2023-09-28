using System;
using System.Collections.Generic;
using System.Text;

namespace MyLab.SmevClient.Crypt
{
    internal static partial class Interop
    {
        public class LastErrorException : Exception
        {
            public LastErrorException() :
                base(GetMessage(Interop.GetLastError()))
            {
            }

            static string GetMessage(int errorCode)
            {
                var buffer = new StringBuilder(512);
                var result = Interop.FormatMessage(
                    Interop.Consts.FORMAT_MESSAGE_IGNORE_INSERTS
                    | Interop.Consts.FORMAT_MESSAGE_FROM_SYSTEM
                    | Interop.Consts.FORMAT_MESSAGE_FROM_HMODULE,
                    IntPtr.Zero, errorCode, 0, buffer, buffer.Capacity, IntPtr.Zero);
                if (result != 0)
                {
                    return buffer.ToString();
                }

                return "Unknown error code: 0x" + errorCode.ToString("X8");
            }
        }
    }
}
