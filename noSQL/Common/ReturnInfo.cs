using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace noSQL.Common
{
    public class ReturnInfo
    {
        public bool Status { get; private set;}
        public string Message { get; private set; }
        public ReturnInfo(bool status, string message = "")
        {
            Status = status;
            Message = message;
        }
    }
}
