using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Сryptographer
{
    class ResultObjectFile : IResultObject
    {
        /// <summary>
        /// Method for checking existing files. 
        /// Returns true, if file exists, and false, if not.  
        ///  </summary>
        /// <param name="strPathToFile"></param>
        /// <returns></returns>
        public bool CheckObjectExist(string strPathToFile)
        {
            FileInfo targetFile = new FileInfo(strPathToFile);
            return targetFile.Exists;
        }

        /// <summary>
        /// Method for writing array of encorypted/decrypted bytes to its own existing file 
        /// </summary>
        /// <param name="strPathToFile"></param> - path to existing file 
        /// <param name="arrBytes"></param> - array of encorypted/decrypted bytes 
        /// <returns></returns>
        public bool SaveCurrentObject(string strPathToFile, byte[] arrBytes)
        {
            // Check arguments.
            if (strPathToFile == null || strPathToFile.Length <= 0)
                throw new СryptographerException("Plain path for saving current object", strPathToFile, DateTime.Now);
            if (arrBytes == null || arrBytes.Length <= 0)
                throw new СryptographerException("Plain data for saving current object", "Source is empty", DateTime.Now);

            using (FileStream fs = new FileStream(strPathToFile, FileMode.Truncate, FileAccess.Write))
            {
                BinaryWriter binWriterToFile = new BinaryWriter(fs);
                binWriterToFile.Write(arrBytes);
            }
            return true;
        }

        /// <summary>
        /// Method for writing array of encorypted/decrypted bytes to new file 
        /// </summary>
        /// <param name="strPathToFile"></param> - new files name 
        /// <param name="arrBytes"></param> - array of encorypted/decrypted bytes 
        /// <returns></returns>
        public bool SaveNewObject(string strPathToFile, byte[] arrBytes)
        {
            if (strPathToFile == null || strPathToFile.Length <= 0)
                throw new СryptographerException("Plain path for saving new object", strPathToFile, DateTime.Now);
            if (arrBytes == null || arrBytes.Length <= 0)
                throw new СryptographerException("Plain data for saving new object", "Source is empty", DateTime.Now);

            //Console.WriteLine(Encoding.Unicode.GetString(arrBytes));
            if (!CheckObjectExist(strPathToFile))
            {
                throw new СryptographerException("File with such address already exists", strPathToFile, DateTime.Now);
            }
            using (FileStream fs = new FileStream(strPathToFile, FileMode.Create, FileAccess.Write))
            {
                BinaryWriter binWriterToFile = new BinaryWriter(fs);
                binWriterToFile.Write(arrBytes);
            }
            return true;
        }

        public bool SaveNewObject(string strPathToFile, string strBytes)
        {
            if (strPathToFile == null || strPathToFile.Length <= 0)
                throw new СryptographerException("Plain path for saving new object", strPathToFile, DateTime.Now);
            if (strBytes == null || strBytes.Length <= 0)
                throw new СryptographerException("Plain data for saving new object", "Source is empty", DateTime.Now);

            if (!CheckObjectExist(strPathToFile))
            {
                throw new СryptographerException("File with such address already exists", strPathToFile, DateTime.Now);
            }
            using (FileStream fs = new FileStream(strPathToFile, FileMode.Create, FileAccess.Write))
            {
                BinaryWriter binWriterToFile = new BinaryWriter(fs);
                binWriterToFile.Write(Encoding.Unicode.GetBytes(strBytes));
            }
            return true;
        }
    }
}
