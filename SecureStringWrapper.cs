using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Utilities
{
    public class SecureStringWrapper : IDisposable
    {
        private readonly Encoding encoding;
        private readonly SecureString secureString;

        private byte[] _bytes = null;
        private bool _disposed = false;

        public SecureStringWrapper(SecureString secureString) : this(secureString, Encoding.UTF8)
        {

        }

        public SecureStringWrapper(SecureString secureString, Encoding encoding)
        {
            this.encoding = encoding ?? Encoding.UTF8;
            this.secureString = secureString;
        }

        public unsafe byte[] ToByteArray()
        {

            int maxLength = encoding.GetMaxByteCount(secureString.Length);

            IntPtr bytes = IntPtr.Zero;
            IntPtr str = IntPtr.Zero;

            try
            {
                bytes = Marshal.AllocHGlobal(maxLength);
                str = Marshal.SecureStringToBSTR(secureString);

                char* chars = (char*)str.ToPointer();
                byte* bptr = (byte*)bytes.ToPointer();
                int len = encoding.GetBytes(chars, secureString.Length, bptr, maxLength);

                _bytes = new byte[len];
                for (int i = 0; i < len; ++i)
                {
                    _bytes[i] = *bptr;
                    bptr++;
                }

                return _bytes;
            }
            finally
            {
                if (bytes != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(bytes);
                }
                if (str != IntPtr.Zero)
                {
                    Marshal.ZeroFreeBSTR(str);
                }
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                Destroy();
                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }

        private void Destroy()
        {
            if (_bytes == null) { return; }

            for (int i = 0; i < _bytes.Length; i++)
            {
                _bytes[i] = 0;
            }
            _bytes = null;
        }

        ~SecureStringWrapper()
        {
            Dispose();
        }
    }
}
