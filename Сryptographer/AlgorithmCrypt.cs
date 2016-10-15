using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Сryptographer
{
    /// <summary>
    /// abstract class of algorithm and method of encryption or decryption 
    /// contains of methods which are independent of data sources and types of writing data 
    /// works just with array of crypted bytes 
    /// </summary>
    abstract class AlgorithmCrypt
    {
        public abstract byte[] Encrypt(byte[] dataForEncrypt, byte[] key, byte[] hash);

        public abstract byte[] Decrypt(byte[] dataForEncrypt, byte[] key, byte[] hash);

        public abstract byte[] GetPassword(byte[] data);

        public abstract byte[] GetHashFile(byte[] data);

        /// <summary>
        /// Method puts word "Encrypted" and hash-code to the end of the array of encrypted bytes
        /// returns array with this word - sign of encryption 
        /// </summary>
        /// <param name="arrBytes"></param>
        /// <returns></returns>
        public void CreateEncryptedSign(ref byte[] arrBytes, byte[] arrHashFile, byte[] arrHashKey)
        {
            // Check arguments.
            if (arrBytes == null || arrBytes.Length <= 0)
                throw new СryptographerException("Plain data for creation encrypted sign", "Source is empty", DateTime.Now);
            if (arrHashFile == null || arrHashFile.Length <= 0)
                throw new СryptographerException("HashFile data for creation encrypted sign", "HashFile is empty", DateTime.Now);
            if (arrHashKey == null || arrHashKey.Length <= 0)
                throw new СryptographerException("Plain key for creation encrypted sign", "Key value is empty", DateTime.Now);
            
            // Creation of string sign "Encrypted"
            byte[] arrSign = Encoding.Unicode.GetBytes("Encrypted"); 

            // Addition of word "Encrypted"(+18) to the encrypted bytes array 
            Array.Resize(ref arrBytes, arrBytes.Length+arrSign.Length);
            for (int i = 0; i < arrSign.Length; i++)
            {
                arrBytes[arrBytes.Length-1 - i] = arrSign[arrSign.Length-1 - i]; 
            }

            // Addition of file's Hash(+16) to this array 
            Array.Resize(ref arrBytes, arrBytes.Length + arrHashFile.Length);
            for (int i = 0; i < arrHashFile.Length; i++)
            {
                arrBytes[arrBytes.Length -1- i] = arrHashFile[arrHashFile.Length-1 - i];
            }

            // Addition of key's Hash(+16) to this array 
            Array.Resize(ref arrBytes, arrBytes.Length + arrHashKey.Length);
            for (int i = 0; i < arrHashKey.Length; i++)
            {
                arrBytes[arrBytes.Length-1 - i] = arrHashKey[arrHashKey.Length-1 - i];
            }
        }

        /// <summary>
        /// Method deletes word "Encrypted" and hash code from the end of the array of encrypted bytes
        /// returns array without any sign of encryption. Array is ready for decryption  
        /// </summary>
        /// <param name="arrBytes"></param>
        /// <returns></returns>
        public byte[] DeleteEncryptedSign(byte[] arrBytes, int size)
        {
            // Check arguments.
            if (arrBytes == null || arrBytes.Length <= 0)
                throw new СryptographerException("Plain data for deleting encrypted sign", "Source is empty", DateTime.Now);
            if (size  <= 0)
                throw new СryptographerException("Plain size for creation encrypted sign", "Size of sign for deleting is empty", DateTime.Now);

            // Creation of string sign "Encrypted"X2(18)+16 hash
            Array.Resize(ref arrBytes, arrBytes.Length-size);
            return arrBytes;
        }

        /// <summary>
        /// Method receives byte array from file and checks:
        /// is it encrypted or not
        /// Returns true is encrypted 
        /// </summary>
        /// <param name="arrBytes"></param>
        /// <returns></returns>
        public bool IsEncrypted(byte[] arrBytes, int sizeOfSigns)
        {
            // Check arguments.
            if (arrBytes == null || arrBytes.Length <= 0)
                throw new СryptographerException("Plain data for checking encrypted sign", "Source is empty", DateTime.Now);
            if (sizeOfSigns <= 0)
                throw new СryptographerException("Plain size for checking encrypted sign", "Size of sign for deleting is empty", DateTime.Now);

            if (arrBytes.Length < sizeOfSigns)
            {
                return false; 
            }
            byte[] arrStrEncrypted = Encoding.Unicode.GetBytes("Encrypted");
            for (int i = 0; i < arrStrEncrypted.Length; i++)
            {
                if (arrBytes[arrBytes.Length - sizeOfSigns + i] != arrStrEncrypted[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Method checks hashes of file and key
        /// Returns true if checking is correct 
        /// </summary>
        /// <param name="arrForChecking"></param>
        /// <param name="arrHashFile"></param>
        /// <param name="arrHashKey"></param>
        /// <returns></returns>
        public bool PasswordCheck(byte[] arrForChecking, byte[] arrHashFile, byte[] arrHashKey)
        {
            // Check arguments.
            if (arrForChecking == null || arrForChecking.Length <= 0)
                throw new СryptographerException("Plain data for checking key and hash", "Source is empty", DateTime.Now);
            if (arrHashFile == null || arrHashFile.Length <= 0)
                throw new СryptographerException("HashFile data for checking key and hash", "HashFile is empty", DateTime.Now);
            if (arrHashKey == null || arrHashKey.Length <= 0)
                throw new СryptographerException("Plain key for checking key and hash", "Key value is empty", DateTime.Now);

            // comparing of hashes of file 
            for (int i = 0; i < arrHashFile.Length; i++)
            {
                if (arrForChecking[arrForChecking.Length - arrHashKey.Length - 1 - i] != arrHashFile[arrHashFile.Length - 1 - i])
                {
                    return false;
                }
            }
            // comparing of key's hashes 
            for (int i = 0; i < arrHashKey.Length; i++)
            {
                if (arrForChecking[arrForChecking.Length-1 - i] != arrHashKey[arrHashKey.Length-1 - i])
                {
                    return false;
                }
            }
            return true; 
        }
    }
}
