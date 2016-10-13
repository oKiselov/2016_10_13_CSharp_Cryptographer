using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Сryptographer
{
    /// <summary>
    /// Потоковый шифр RC4 был создан Рональдом Ривестом, сотрудником компании «RSA Security», в 1987 году. 
    /// Сокращение «RC4» официально обозначает «Rivest cipher 4» или «шифр Ривеста» 
    /// («4» - номер версии; см. RC2, RC5, RC6; RC1 никогда не публиковался; RC3 разрабатывался, 
    /// но в нём была найдена уязвимость), но его часто считают сокращением от «Ron’s code» («код Рона»)[2].
    /// </summary>
    class MyRC4:AlgorithmCrypt
    {
        // массив байт для генерации значенй для перестановки 
        byte[] S_block = new byte[256];
        // счетчики 
        int x_counter = 0;
        int y_counter = 0;


        /// <summary>
        /// Метод init нужно вызвать перед шифровкой/расшифровкой, когда известен ключ. 
        /// Можно сделать это в конструкторе:
        /// </summary>
        /// <param name="key"></param>
        public MyRC4(){}

        public override byte[] GetPassword(byte[] data)
        {

            if (data == null || data.Length <= 0)
                throw new СryptographerException("Plain data for getting password", "Source for creation of password is empty", DateTime.Now);

            MD5 algorithmMd5 = MD5.Create();
            // Creation of hash of current password  
            data = algorithmMd5.ComputeHash(data);
            return data;
        }

        public override byte[] GetHashFile(byte[] data)
        {
            // Check arguments.
            if (data == null || data.Length <= 0)
                throw new СryptographerException("Plain data for getting hashfile", "Source for creation of fileshash is empty", DateTime.Now);
            // Creation of hash of current array of encrypted bytes 
            MD5 algorithmMd5 = MD5.Create();
            data = algorithmMd5.ComputeHash(data);
            return data; 
        }

        /// <summary>
        /// Алгоритм также известен как «key-scheduling algorithm» или «KSA». 
        /// Этот алгоритм использует ключ, подаваемый на вход пользователем, сохранённый в Key, и имеющий длину L байт. 
        /// Инициализация начинается с заполнения массива S, далее этот массив перемешивается путём перестановок, 
        /// определяемых ключом. Так как только одно действие выполняется над S, то должно выполняться утверждение, 
        /// что S всегда содержит один набор значений , который был дан при первоначальной инициализации (S[i] := i).
        /// </summary>
        /// <param name="key"></param>
        private void KeyInitialize(byte[] key)
        {
            if (key == null || key.Length <= 0)
                throw new СryptographerException("Plain key for encryption", "Key value is empty", DateTime.Now);
            
            int keyLength = key.Length;

            for (int i = 0; i < 256; i++)
            {
                S_block[i] = (byte)i;
            }

            int j = 0;
            for (int i = 0; i < 256; i++)
            {
                j = (j + S_block[i] + key[i % keyLength]) % 256;
                byte temp = S_block[i];
                S_block[i] = S_block[j];
                S_block[j] = temp;
            }
        }

        /// <summary>
        /// (pseudo-random generation algorithm, PRGA)
        /// Эта часть алгоритма называется генератором псевдослучайной последовательности. 
        /// Генератор ключевого потока RC4 переставляет значения, хранящиеся в S. 
        /// В одном цикле RC4 определяется одно n-битное слово K из ключевого потока. 
        /// В дальнейшем ключевое слово будет сложено по модулю два с исходным текстом, 
        /// которое пользователь хочет зашифровать, и получен зашифрованный текст.
        /// </summary>
        /// <returns></returns>
        private byte KeyItemForXOR()
        {
            x_counter = (x_counter + 1) % 256;
            y_counter = (y_counter + S_block[x_counter]) % 256;
            byte temp = S_block[x_counter];
            S_block[x_counter] = S_block[y_counter];
            S_block[y_counter] = temp;

            return S_block[(S_block[x_counter] + S_block[y_counter]) % 256];
        }

        /// <summary>
        /// Метод шифрования 
        /// </summary>
        /// <param name="dataForEncrypt"></param>
        /// <param name="key"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public override byte[] Encrypt(byte[] dataForEncrypt, byte[] key, byte[] hash=null)
        {
            // Check arguments.
            if (dataForEncrypt == null || dataForEncrypt.Length <= 0)
                throw new СryptographerException("Plain data for encryption", "Source is empty", DateTime.Now);
            if (key == null || key.Length <= 0)
                throw new СryptographerException("Plain key for encryption", "Key value is empty", DateTime.Now);

            KeyInitialize(key);
            
            byte temp = 0; 

            for (int m = 0; m < dataForEncrypt.Length; m++)
            {
                dataForEncrypt[m] = (byte)(dataForEncrypt[m] ^ KeyItemForXOR());
            }
            return dataForEncrypt;
        }

        public override byte[] Decrypt(byte[] dataForEncrypt, byte[] key , byte[] hash = null)
        {
            return Encrypt(dataForEncrypt, key, hash);
        }
    }
}
