using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Сryptographer
{
    /// <summary>
    /// Interface for creation of results of en-decryption activity
    /// </summary>
    interface IResultObject
    {
        bool CheckObjectExist(string strPathToFile);

        bool SaveCurrentObject(string strPathToFile, byte[] arrBytes);

        bool SaveNewObject(string strPathToFile, byte[] arrBytes);

        bool SaveNewObject(string strPathToFile, string strBytes); 
    }
}
