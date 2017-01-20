using System;
using System.IO;
using System.Security;

namespace Utilities
{
    public class UtilityMethods
    {
        public static string GetFile()
        {
            Console.WriteLine("Please enter the filepath of the file to be encrypted");

            string path;
            while (true)
            {
                path = Console.ReadLine();
                if (!File.Exists(path))
                {
                    Console.WriteLine("That file does not exist, or you do not have adequate permissions to access the file. Please try again");
                }
                else
                {
                    Console.WriteLine();
                    return path;
                }
            }
        }

        public static SecureString GetPassword()
        {
            SecureString firstPassword = new SecureString();
            SecureString secondPassword = new SecureString();
            ConsoleKeyInfo info;

            bool firstEntryDone = false;
            while (true)
            {
                while (!firstEntryDone)
                {
                    info = Console.ReadKey(true);
                    if (info.Key != ConsoleKey.Enter)
                    {
                        if (info.Key != ConsoleKey.Backspace)
                        {
                            firstPassword.AppendChar(info.KeyChar);
                            Console.Write('*');
                        }
                        else
                        {
                            firstPassword.RemoveAt(firstPassword.Length - 1);
                        }
                    }
                    else
                    {
                        Console.WriteLine();
                        firstEntryDone = true;

                        Console.WriteLine("To ensure accuracy, please re-enter");
                    }
                }

                info = Console.ReadKey(true);
                if (info.Key != ConsoleKey.Enter)
                {
                    if (info.Key != ConsoleKey.Backspace)
                    {
                        secondPassword.AppendChar(info.KeyChar);
                        Console.Write('*');
                    }
                    else
                    {
                        Console.WriteLine();
                        secondPassword.RemoveAt(secondPassword.Length - 1);
                    }
                }
                else
                {
                    if (!firstPassword.IsEqualTo(secondPassword))
                    {
                        firstPassword.Clear();
                        secondPassword.Clear();
                        firstEntryDone = false;

                        Console.WriteLine();
                        Console.WriteLine("Passwords did not match. Please Try again");
                    }
                    else
                    {
                        Console.WriteLine();
                        return firstPassword;
                    }
                }
            }
        }

        public static void DestroyBytes(byte[] bytes)
        {
            if (bytes == null)
            {
                return;
            }

            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = 0;
            }
            bytes = null;
        }
    }
}
