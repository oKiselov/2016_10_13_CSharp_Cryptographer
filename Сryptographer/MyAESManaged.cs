using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Сryptographer
{
    class MyAESManaged:AlgorithmCrypt
    {
        private AesManaged aesAlg;

        public MyAESManaged()
        {
            aesAlg = new AesManaged();
        }
        
        public override byte[] GetPassword(byte[] data = null)
        {
            return aesAlg.Key;
        }

        public override byte[] GetHashFile(byte[] data = null)
        {
            return aesAlg.IV;
        }

        public override byte[] Encrypt(byte[] dataForEncrypt, byte[] key, byte[] IV)
        {
            // Check arguments.
            if (dataForEncrypt == null || dataForEncrypt.Length <= 0)
                throw new СryptographerException("Plain data for encryption", "Source is empty", DateTime.Now);
            if (key == null || key.Length <= 0)
                throw new СryptographerException("Plain key for encryption", "Key value is empty", DateTime.Now);
            if (IV == null || IV.Length <= 0)
                throw new СryptographerException("IV data for encryption", "IV is empty", DateTime.Now);
            byte[] encrypted;
            // Create an AesManaged object
            // with the specified key and IV.
            using (aesAlg)
            {
                aesAlg.Key = key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (BinaryWriter swEncrypt = new BinaryWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(dataForEncrypt);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }
        
        public override byte[] Decrypt(byte[] dataForEncrypt, byte[] key, byte[] IV)
        {
            // Check arguments.
            if (dataForEncrypt == null || dataForEncrypt.Length <= 0)
                throw new СryptographerException("Plain data for encryption", "Source is empty", DateTime.Now);
            if (key == null || key.Length <= 0)
                throw new СryptographerException("Plain key for encryption", "Key value is empty", DateTime.Now);
            if (IV == null || IV.Length <= 0)
                throw new СryptographerException("IV data for encryption", "IV is empty", DateTime.Now);

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an AesManaged object
            // with the specified key and IV.
            using (aesAlg)
            {
                aesAlg.Key = key;
                aesAlg.IV = IV;
                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(dataForEncrypt))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (BinaryReader srDecrypt = new BinaryReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            dataForEncrypt = srDecrypt.ReadBytes(dataForEncrypt.Length);
                        }
                    }
                }
            }
            return dataForEncrypt;
        }
    }
}
