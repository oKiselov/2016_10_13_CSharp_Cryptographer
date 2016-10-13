using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Сryptographer
{
    class Program
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            //log.Error("Hi");
            //////////////////////////////////////////////
            try
            {
                SourceReaderFromFile srFile = new SourceReaderFromFile();
                ResultObjectFile roFile = new ResultObjectFile();

                Console.WriteLine("Enter algorithm: \r\n1 - for RC4; \r\n2 - for AES.");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Enter the path to file you want to encrypt: ");
                        string strPath = Console.ReadLine();
                        byte[] byteForCheck = srFile.ReadForCheck(strPath, (int) SizeOfSigns.signRC4);
                        MyRC4 algorithmCrypt = new MyRC4();
                        Console.WriteLine("File is crypted: {0}",
                            algorithmCrypt.IsEncrypted(byteForCheck, (int) SizeOfSigns.signRC4));

                        //byte[] result = srFile.ReadSource(strPath);
                        //Console.WriteLine(Encoding.Unicode.GetString(Full));

                        if (!algorithmCrypt.IsEncrypted(byteForCheck, (int) SizeOfSigns.signRC4))
                        {
                            Console.WriteLine("Enter the password for encryption: ");
                            string strPassword = Console.ReadLine();
                            byte[] arrHashKey = algorithmCrypt.GetPassword(Encoding.Unicode.GetBytes(strPassword));
                            byte[] result = srFile.ReadSource(strPath);

                            result = algorithmCrypt.Encrypt(result, arrHashKey);
                            //Console.WriteLine(Encoding.Unicode.GetString(arrFromFile));

                            byte[] arrHashFile = algorithmCrypt.GetHashFile(result);

                            algorithmCrypt.CreateEncryptedSign(ref result, arrHashFile, arrHashKey);
                            Console.WriteLine(Encoding.Unicode.GetString(result));
                            roFile.SaveNewObject("D:\\134.txt", result);

                            //Console.WriteLine("File hash: {0}\r\nKey hash: {1}", sFile, sKey);
                            Console.WriteLine("You will get your password and hash-files code in participant files.");
                            roFile.SaveNewObject("HFileRC4.dat", arrHashFile);
                            roFile.SaveNewObject("HKeyRC4.dat", arrHashKey);
                        }
                        else
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
                                roFile.SaveNewObject("D:\\25.png", result);
                            }
                            else
                            {
                                Console.WriteLine("Password is invalid!");
                            }
                        }
                        break;

                    case "2":
                        Console.WriteLine("Enter the path to file you want to encrypt: ");
                        string strPathAES = Console.ReadLine();
                        byte[] byteForCheckAES = srFile.ReadForCheck(strPathAES, (int) SizeOfSigns.signAES);
                        MyAESManaged algorithmAES = new MyAESManaged();
                        Console.WriteLine("File is crypted: {0}",
                            algorithmAES.IsEncrypted(byteForCheckAES, (int) SizeOfSigns.signAES));

                        //byte[] result = srFile.ReadSource(strPath);
                        //Console.WriteLine(Encoding.Unicode.GetString(Full));

                        if (!algorithmAES.IsEncrypted(byteForCheckAES, (int) SizeOfSigns.signAES))
                        {
                            Console.WriteLine("You will get your password and hash-files code in participant files.");
                            byte[] key = algorithmAES.GetPassword();
                            byte[] IV = algorithmAES.GetHashFile();
                            roFile.SaveNewObject("HFileAES.dat", IV);
                            roFile.SaveNewObject("HKeyAES.dat", key);
                            byte[] result = srFile.ReadSource(strPathAES);
                            Console.WriteLine(result.Length);

                            result = algorithmAES.Encrypt(result, key, IV);
                            Console.WriteLine(Encoding.Unicode.GetString(result));
                            Console.WriteLine(result.Length);
                            algorithmAES.CreateEncryptedSign(ref result, IV, key);
                            roFile.SaveNewObject("D:\\134.txt", result);
                        }
                        else
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
                                roFile.SaveNewObject("D:\\25.png", result);
                            }
                            else
                            {
                                Console.WriteLine("Password is invalid!");
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (CryptographicException ex)
            {
                log.Error("Error", ex);
                Console.WriteLine(ex.Message);
            }
            catch (System.Exception ex)
            {
                log.Error("Error", ex);
                Console.WriteLine(ex.Message);
            }
            catch
            {
                log.Error("Unhandled Error");
                Console.WriteLine("Unhandled Exception");
            }
            finally
            {
                Console.WriteLine("Closed");
            }
        }
    }
}
