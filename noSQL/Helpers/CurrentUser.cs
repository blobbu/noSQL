using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace noSQL.Helpers
{
    public enum UserRoles
    {
        Guest,
        User,
        Admin
    }
    public class CurrentUser
    {
        public string Name { get; set; }
        public UserRoles Role { get; set; }
    }
}
