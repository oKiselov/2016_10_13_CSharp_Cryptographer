﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Сryptographer
{
    /// <summary>
    /// interface which works with data source 
    /// </summary>
    interface ISourceReader
    {
        byte[] ReadForCheck(string strPath, int size);

        bool CheckSourceExist(string strPathToFile);

        byte[] ReadSource(string strPathToFile);
    }
}
