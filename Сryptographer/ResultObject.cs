using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Сryptographer
{
    interface IResultObject
    {
        bool CheckObjectExist(string strPathToFile);

        bool SaveCurrentObject(string strPathToFile, byte[] arrBytes);

        bool SaveNewObject(string strPathToFile, byte[] arrBytes);

        bool SaveNewObject(string strPathToFile, string strBytes); 
    }
}
