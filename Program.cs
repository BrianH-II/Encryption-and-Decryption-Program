using System;
using Encryptor;
using Decryptor;


namespace Cryptor
{
    /*
     *  This program will encrypt / decrypt files using the AesManaged class in conjunction with a CryptoStream.
     *  Due to the nature of CBC encryption, encrypting / decrypting large files does take a significant ammount of time.
     *  
     *  This program uses the SecureString class to store the user's password to eliminate many of the security risks associated with the string class.
     *  For increased security, the user-entered password is salted with a pseudo-random string before being hashed.
     *  This program also ensures that all sensitive data is eliminated form memory when finished.
     *  
     *  The user can choose to encrypt / decrypt multiple files in one session.
     */ 

    class Program
    {
        static bool outBreak = false;

        static void Main(string[] args)
        {
            Console.WriteLine("Type 'E' to encrypt, or 'D' to decrypt");
            Console.WriteLine("To end, click Enter");

            do
            {
                bool validEntry = true;
                ConsoleKeyInfo key = Console.ReadKey();
                do
                {
                    if (key.Key == ConsoleKey.E)
                    {
                        Console.Write("\r\n \r\n");

                        EncryptorProgram.Main(new string[0]);
                    }
                    else if (key.Key == ConsoleKey.D)
                    {
                        Console.Write("\r\n \r\n");

                        DecryptorProgram.Main(new string[0]);
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        outBreak = true;
                        break;
                    }
                    else
                    {
                        validEntry = false;
                        Console.WriteLine("\r\n \r\nInvalid character entered. Please try again. \r\nType 'E' to encrypt, or 'D' to decrypt");

                        key = Console.ReadKey();
                    }
                }
                while (!validEntry);

                Console.WriteLine("Operation complete. To perform another operation, type 'E' or 'D'");
                Console.WriteLine("If finished, simply click Enter");
            }
            while (!outBreak);
        }
    }
}
