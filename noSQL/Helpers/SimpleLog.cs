using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace noSQL.Helpers
{
    public class SimpleLog
    {
        public string Time { get; set; }
        public string UserName { get; set; }
        public UserRoles UserRole { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
    }
}
