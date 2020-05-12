using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using noSQL.Helpers;
using noSQL.Controllers;

namespace noSQL.Filters
{
    public class AuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.Controller as BaseController;
            var user = controller.CurrentUser;

            if(user.Name == "" || user.Role == UserRoles.Guest)
            {
                throw new Exception("Błąd autoryzacji");
            }
        }
        
    }
}
