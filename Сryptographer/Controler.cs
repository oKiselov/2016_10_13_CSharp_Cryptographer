using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Сryptographer
{
    /// <summary>
    /// Class for viewing the main and dropped menues of Сryptographer
    /// </summary>
    class Controler
    {
        /// <summary>
        /// array of strings - options which are avaliable in Сryptographer
        /// </summary>
        string[] argsStrings;

        public Controler(params string[] argStrings)
        {
            argsStrings = new string[argStrings.Length];
            argStrings.CopyTo(argsStrings, 0);
        }

        public Controler()
        {
        }

        /// <summary>
        /// Main method of performance the main and dropped menues of Сryptographer
        /// </summary>
        /// <param name="arrStringOption"></param>
        /// <returns></returns>
        private int Menu(params string[] arrStringOption)
        {
            int choice = 0;
            while (true)
            {
                ConsoleColor backGround = ConsoleColor.Gray;
                ConsoleColor foreGround = ConsoleColor.Black;

                Console.Clear();
                Console.WriteLine("Cryptographer.v.1.0 (c) 2016, Kiselov O.");
                Console.WriteLine("Choose working mode: ");
                for (int i = 0; i < argsStrings.Length; ++i)
                {
                    if (i == choice)
                    {
                        Console.BackgroundColor = foreGround;
                        Console.ForegroundColor = backGround;
                    }
                    else
                    {
                        Console.BackgroundColor = backGround;
                        Console.ForegroundColor = foreGround;
                    }
                    Console.WriteLine(argsStrings[i]);
                }

                ConsoleKey ki = Console.ReadKey().Key;

                switch (ki)
                {
                    case ConsoleKey.UpArrow:
                        --choice;
                        break;
                    case ConsoleKey.DownArrow:
                        ++choice;
                        break;
                    case ConsoleKey.Enter:
                        return choice;
                }
                if (choice < 0)
                {
                    choice = argsStrings.Length - 1;
                }
                if (choice >= argsStrings.Length)
                {
                    choice = 0;
                }
            }
        }

        /// <summary>
        /// Menu for choosing mode of work - encrypt or decrypt 
        /// </summary>
        public void MenuChooseMode()
        {
            ConsoleColor backGround = ConsoleColor.Gray;
            ConsoleColor foreGround = ConsoleColor.Black;
            Console.Clear();
            Console.WriteLine("Cryptographer.v.1.0 (c) 2016, Kiselov O.");
            Console.WriteLine("Choose working mode: ");
            Controler menuMode = new Controler("Encryption", "Decryption", "Exit"); 
            switch (menuMode.Menu(argsStrings))
            {
                case (int) OptionMode.Encryption:
                    MenuEncryption();
                    break;
                case (int) OptionMode.Decryption:
                    MenuDecryption();
                    break;
                case (int) OptionMode.Exit:
                    return;
                default:
                    break;
            }
        }

        /// <summary>
        /// main encryption menu
        /// </summary>
        public void MenuEncryption()
        {
            ConsoleColor backGround = ConsoleColor.Gray;
            ConsoleColor foreGround = ConsoleColor.Black;
            Console.Clear();
            Console.WriteLine("Cryptographer.v.1.0 (c) 2016, Kiselov O.");
            Console.WriteLine("Choose needed algorithm: ");
            Controler menuEncrypt = new Controler("RC4", "AES", "Return to main menu", "Exit");
            switch (menuEncrypt.Menu(argsStrings))
            {
                case (int) OptionAlgorithm.RC4:
                    MenuRC4Encryption();
                    break;
                case (int) OptionAlgorithm.AES:
                    MenuAESEncryption();
                    break;
                case (int) OptionAlgorithm.Return:
                    MenuChooseMode();
                    break;
                case (int) OptionAlgorithm.Exit:
                    return;
                default:
                    break;
            }
        }

        /// <summary>
        /// main decryption menu
        /// </summary>
        public void MenuDecryption()
        {
            ConsoleColor backGround = ConsoleColor.Gray;
            ConsoleColor foreGround = ConsoleColor.Black;
            Console.Clear();
            Console.WriteLine("Cryptographer.v.1.0 (c) 2016, Kiselov O.");
            Console.WriteLine("Choose needed algorithm: ");
            Controler menuDecrypt = new Controler("RC4", "AES", "Return to main menu", "Exit");
            switch (menuDecrypt.Menu(argsStrings))
            {
                case (int) OptionAlgorithm.RC4:
                    MenuRC4Decryption();
                    break;
                case (int) OptionAlgorithm.AES:
                    MenuAESDecryption();
                    break;
                case (int) OptionAlgorithm.Return:
                    MenuChooseMode();
                    break;
                case (int) OptionAlgorithm.Exit:
                    return;
                default:
                    break;
            }
        }

        /// <summary>
        /// menu for using the RC4 encryption algorithm
        /// </summary>
        public void MenuRC4Encryption()
        {
            ConsoleColor backGround = ConsoleColor.Gray;
            ConsoleColor foreGround = ConsoleColor.Black;
            Console.Clear();
            Console.WriteLine("Cryptographer.v.1.0 (c) 2016, Kiselov O.");

            SourceReaderFromFile srFile = new SourceReaderFromFile();
            ResultObjectFile roFile = new ResultObjectFile();
            Console.WriteLine("Enter the path to file you want to encrypt: ");
            string strPath = Console.ReadLine();
            byte[] byteForCheck = srFile.ReadForCheck(strPath, (int) SizeOfSigns.signRC4);
            MyRC4 algorithmCrypt = new MyRC4();
            if (!algorithmCrypt.IsEncrypted(byteForCheck, (int) SizeOfSigns.signRC4))
            {
                Console.WriteLine("Enter the password for encryption: ");
                string strPassword = Console.ReadLine();
                byte[] arrHashKey = algorithmCrypt.GetPassword(Encoding.Unicode.GetBytes(strPassword));
                byte[] result = srFile.ReadSource(strPath);

                result = algorithmCrypt.Encrypt(result, arrHashKey);

                byte[] arrHashFile = algorithmCrypt.GetHashFile(result);

                algorithmCrypt.CreateEncryptedSign(ref result, arrHashFile, arrHashKey);
                Console.WriteLine(
                    "Press 1 - to save changes in current file, \r\n2 - to save new file with encrypted data: ");
                switch (Console.ReadLine())
                {
                    case "1":
                        roFile.SaveCurrentObject(strPath, result);
                        break;
                    case "2":
                        Console.WriteLine("Enter new path to file: ");
                        roFile.SaveNewObject(Console.ReadLine(), result);
                        break;
                    default:
                        throw new СryptographerException("Incorrect choose for file saving",
                            "You may choose between 1 and 2", DateTime.Now);
                        break;
                }
                Console.WriteLine(
                    "You will get your password and hash-files code \r\nin separate files in the root directory.");
                roFile.SaveNewObject("HFileRC4.dat", arrHashFile);
                roFile.SaveNewObject("HKeyRC4.dat", arrHashKey);
            }
            else
            {
                throw new СryptographerException("Current file is already encrypted", "You may choose unencrypted file",
                    DateTime.Now);
            }
        }

        /// <summary>
        /// menu for using the RC4 decryption algorithm 
        /// </summary>
        public void MenuRC4Decryption()
        {
            ConsoleColor backGround = ConsoleColor.Gray;
            ConsoleColor foreGround = ConsoleColor.Black;
            Console.Clear();
            Console.WriteLine("Cryptographer.v.1.0 (c) 2016, Kiselov O.");

            SourceReaderFromFile srFile = new SourceReaderFromFile();
            ResultObjectFile roFile = new ResultObjectFile();
            Console.WriteLine("Enter the path to file you want to decrypt: ");
            string strPath = Console.ReadLine();
            byte[] byteForCheck = srFile.ReadForCheck(strPath, (int) SizeOfSigns.signRC4);
            MyRC4 algorithmCrypt = new MyRC4();

            if (algorithmCrypt.IsEncrypted(byteForCheck, (int) SizeOfSigns.signRC4))
            {
                Console.WriteLine("Enter password: ");
                byte[] temp = Encoding.Unicode.GetBytes(Console.ReadLine());
                byte[] hashKeyFromUser = algorithmCrypt.GetPassword(temp);
                Console.WriteLine("Enter the path to file with files hashcode: ");
                byte[] hashFileFromUser = srFile.ReadSource(Console.ReadLine());

                if (algorithmCrypt.PasswordCheck(byteForCheck, hashFileFromUser, hashKeyFromUser))
                {
                    byte[] result = srFile.ReadSource(strPath);
                    result = algorithmCrypt.DeleteEncryptedSign(result, (int) SizeOfSigns.signRC4);
                    result = algorithmCrypt.Decrypt(result, hashKeyFromUser);

                    Console.WriteLine(
                        "Press 1 - to save changes in current file, \r\n2 - to save new file with decrypted data: ");
                    switch (Console.ReadLine())
                    {
                        case "1":
                            roFile.SaveCurrentObject(strPath, result);
                            break;
                        case "2":
                            Console.WriteLine("Enter new path to file: ");
                            roFile.SaveNewObject(Console.ReadLine(), result);
                            break;
                        default:
                            throw new СryptographerException("Incorrect choose for file saving",
                                "You may choose between 1 and 2", DateTime.Now);
                            break;
                    }
                }
                else
                {
                    throw new СryptographerException("Invalid password", "You may enter correct password",
                        DateTime.Now);
                }
            }
            else
            {
                throw new СryptographerException("Current file is not encrypted yet", "You may choose encrypted file",
                    DateTime.Now);
            }
        }

        /// <summary>
        /// menu for using the AES encryption algorithm
        /// </summary>
        public void MenuAESEncryption()
        {
            ConsoleColor backGround = ConsoleColor.Gray;
            ConsoleColor foreGround = ConsoleColor.Black;
            Console.Clear();
            Console.WriteLine("Cryptographer.v.1.0 (c) 2016, Kiselov O.");

            SourceReaderFromFile srFile = new SourceReaderFromFile();
            ResultObjectFile roFile = new ResultObjectFile();

            Console.WriteLine("Enter the path to file you want to encrypt: ");
            string strPathAES = Console.ReadLine();
            byte[] byteForCheckAES = srFile.ReadForCheck(strPathAES, (int) SizeOfSigns.signAES);
            MyAESManaged algorithmAES = new MyAESManaged();

            if (!algorithmAES.IsEncrypted(byteForCheckAES, (int) SizeOfSigns.signAES))
            {
                byte[] result = srFile.ReadSource(strPathAES);
                byte[] key = algorithmAES.GetPassword();
                byte[] IV = algorithmAES.GetHashFile();
                result = algorithmAES.Encrypt(result, key, IV);
                algorithmAES.CreateEncryptedSign(ref result, IV, key);
                Console.WriteLine(
                    "Press 1 - to save changes in current file, \r\n2 - to save new file with encrypted data: ");
                switch (Console.ReadLine())
                {
                    case "1":
                        roFile.SaveCurrentObject(strPathAES, result);
                        break;
                    case "2":
                        Console.WriteLine("Enter new path to file: ");
                        roFile.SaveNewObject(Console.ReadLine(), result);
                        break;
                    default:
                        throw new СryptographerException("Incorrect choose for file saving",
                            "You may choose between 1 and 2", DateTime.Now);
                        break;
                }
                Console.WriteLine(
                    "You will get your password and hash-files code \r\nin separate files in the root directory.");
                roFile.SaveNewObject("HFileAES.dat", IV);
                roFile.SaveNewObject("HKeyAES.dat", key);
            }
            else
            {
                throw new СryptographerException("Current file is already encrypted", "You may choose unencrypted file",
                    DateTime.Now);
            }
        }

        /// <summary>
        /// menu for using the AES decryption algorithm
        /// </summary>
        public void MenuAESDecryption()
        {
            ConsoleColor backGround = ConsoleColor.Gray;
            ConsoleColor foreGround = ConsoleColor.Black;
            Console.Clear();
            Console.WriteLine("Cryptographer.v.1.0 (c) 2016, Kiselov O.");

            SourceReaderFromFile srFile = new SourceReaderFromFile();
            ResultObjectFile roFile = new ResultObjectFile();

            Console.WriteLine("Enter the path to file you want to encrypt: ");
            string strPathAES = Console.ReadLine();
            byte[] byteForCheckAES = srFile.ReadForCheck(strPathAES, (int) SizeOfSigns.signAES);
            MyAESManaged algorithmAES = new MyAESManaged();

            if (algorithmAES.IsEncrypted(byteForCheckAES, (int) SizeOfSigns.signAES))
            {
                Console.WriteLine("Enter the path to file with keys hashcode: ");
                byte[] hashKeyFromUser = srFile.ReadSource(Console.ReadLine());
                Console.WriteLine("Enter the path to file with files hashcode: ");
                byte[] hashFileFromUser = srFile.ReadSource(Console.ReadLine());

                if (algorithmAES.PasswordCheck(byteForCheckAES, hashFileFromUser, hashKeyFromUser))
                {
                    byte[] result = srFile.ReadSource(strPathAES);
                    result = algorithmAES.DeleteEncryptedSign(result, (int) SizeOfSigns.signAES);
                    result = algorithmAES.Decrypt(result, hashKeyFromUser, hashFileFromUser);
                    Console.WriteLine(
                        "Press 1 - to save changes in current file, \r\n2 - to save new file with decrypted data: ");
                    switch (Console.ReadLine())
                    {
                        case "1":
                            roFile.SaveCurrentObject(strPathAES, result);
                            break;
                        case "2":
                            Console.WriteLine("Enter new path to file: ");
                            roFile.SaveNewObject(Console.ReadLine(), result);
                            break;
                        default:
                            throw new СryptographerException("Incorrect choose for file saving",
                                "You may choose between 1 and 2", DateTime.Now);
                            break;
                    }
                }
                else
                {
                    throw new СryptographerException("Invalid password", "You may enter correct password",
                        DateTime.Now);
                }
            }
            else
            {
                throw new СryptographerException("Current file is not encrypted yet", "You may choose encrypted file",
                    DateTime.Now);
            }
        }
    }
}
