using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Сryptographer
{
    class SourceReaderFromFile:ISourceReader
    {
        /// <summary>
        /// Method of reading of the part of file for password checking 
        /// 16+16 (2 hashes)+18(string "Encrypted" to array of bytes)
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        public byte[] ReadForCheck(string strPath, int size)
        {
            // Check arguments.
            if (strPath == null || strPath.Length <= 0)
                throw new СryptographerException("Plain path for checking file for encryption", strPath, DateTime.Now);
            if (size <= 0)
                throw new СryptographerException("Plain size for checking file for encryption", "Enter amount of bytes", DateTime.Now);

            if (!CheckSourceExist(strPath))
            {
                throw new СryptographerException("File with such address doesn't exist", strPath, DateTime.Now);
            }

            byte[] arrBytesFromFileRet;
            using (FileStream fs = new FileStream(strPath, FileMode.Open, FileAccess.Read))
            {
                if (fs.Length > size)
                {
                    fs.Position = fs.Length - size;
                    BinaryReader binReaderFromFile = new BinaryReader(fs);
                    arrBytesFromFileRet = binReaderFromFile.ReadBytes(size);
                }
                else
                {
                    arrBytesFromFileRet = new byte[1];
                }
            }
            Console.WriteLine(Encoding.Unicode.GetString(arrBytesFromFileRet));
            return arrBytesFromFileRet;
        }


        /// <summary>
        /// Method for checking existing files. 
        /// Returns true, if file exists, and false, if not.  
        ///  </summary>
        /// <param name="strPathToFile"></param>
        /// <returns></returns>
        public bool CheckSourceExist(string strPathToFile)
        {
            FileInfo targetFile = new FileInfo(strPathToFile);
            return targetFile.Exists;
        }

        /// <summary>
        /// Method for reading array of bytes from current file
        /// Returns array of bytes  
        /// </summary>
        /// <param name="strPathToFile"></param>
        /// <returns></returns>
        public byte[] ReadSource(string strPathToFile)
        {
            // Check arguments.
            if (strPathToFile == null || strPathToFile.Length <= 0)
                throw new СryptographerException("Plain path for reading file for encryption/decryprion", strPathToFile, DateTime.Now);
            
            if (!CheckSourceExist(strPathToFile))
            {
                throw new СryptographerException("File with such address doesn't exist", strPathToFile, DateTime.Now);
            }
            byte[] arrBytesFromFileRet;
            using (FileStream fs = new FileStream(strPathToFile, FileMode.Open, FileAccess.Read))
            {
                fs.Position = 0;
                BinaryReader binReaderFromFile = new BinaryReader(fs);
                arrBytesFromFileRet = binReaderFromFile.ReadBytes((int) fs.Length);
            }
            return arrBytesFromFileRet;
        }
    }
}
