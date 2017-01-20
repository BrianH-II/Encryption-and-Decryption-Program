using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using Utilities;

namespace Decryptor
{
    public class DecryptorProgram
    {
        public static void Main(string[] args)
        {
            string filePath = args[0];

            Console.WriteLine("Enter the decryption password");
            SecureString password = UtilityMethods.GetPassword();
            byte[] passwordBytes;
            using (SecureStringWrapper wrapper = new SecureStringWrapper(password))
            {
                passwordBytes = wrapper.ToByteArray();

                byte[] salt = Encoding.ASCII.GetBytes("M%qQrgp@tKWZ3&!B"); //Salt was generated using the RNGCryptoServiceProvider class
                GCHandle saltHandle = GCHandle.Alloc(salt, GCHandleType.Pinned);

                decryptFile(filePath, passwordBytes, salt);

                UtilityMethods.DestroyBytes(passwordBytes);
                UtilityMethods.DestroyBytes(salt);
                saltHandle.Free();
            }
        }

        private static void decryptFile(string filePath, byte[] password, byte[] salt)
        {
            Rfc2898DeriveBytes rdb = new Rfc2898DeriveBytes(password, salt, 100000);
            AesManaged algorithm = new AesManaged();

            byte[] rgbKey = rdb.GetBytes(algorithm.KeySize / 8);
            byte[] rgbIV = rdb.GetBytes(algorithm.BlockSize / 8);
            GCHandle keyHandle = GCHandle.Alloc(rgbKey, GCHandleType.Pinned);
            GCHandle IVHandle = GCHandle.Alloc(rgbIV, GCHandleType.Pinned);

            ICryptoTransform cryptoAlgorithm = algorithm.CreateDecryptor(rgbKey, rgbIV);

            using (FileStream readStream = File.Open(filePath, FileMode.Open))
            {
                using (FileStream writeStream = new FileStream(filePath.Substring(0, filePath.Length - 4) + ".dec", FileMode.Create, FileAccess.Write))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(writeStream, cryptoAlgorithm, CryptoStreamMode.Write))
                    {
                        while (readStream.Position < readStream.Length)
                        {
                            byte[] buffer = new byte[4096];
                            int amountRead = readStream.Read(buffer, 0, buffer.Length);
                            cryptoStream.Write(buffer, 0, amountRead);
                        }
                        cryptoStream.Flush();
                    }
                }
            }

            UtilityMethods.DestroyBytes(rgbKey);
            UtilityMethods.DestroyBytes(rgbIV);
            keyHandle.Free();
            IVHandle.Free();
        }
    }
}
