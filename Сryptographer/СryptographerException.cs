using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Сryptographer
{
    class СryptographerException:Exception
    {
        private string messageDetails = string.Empty;
        public DateTime ErrorTimeStamp { get; set; }
        public string CauseOfError { get; set; }

        public СryptographerException(string message, string cause, DateTime time)
        {
            messageDetails = message;
            CauseOfError = cause;
            ErrorTimeStamp = time;
        }

        public override string Message
        {
            get { return string.Format("Сryptographer message: {0}", messageDetails); }
        }
    }
}
