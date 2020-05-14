using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using noSQL.Helpers;
using noSQL.Controllers;
using Newtonsoft.Json;

namespace noSQL.Filters
{
    public class Logger : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.Controller as BaseController;
            var user = controller.CurrentUser;

            var controllerName = ((BaseController)context.Controller).ControllerContext.ActionDescriptor.ControllerName;
            var actionName = ((BaseController)context.Controller).ControllerContext.ActionDescriptor.ActionName;

            if (user.Name == "" || user.Role == UserRoles.Guest)
            {
                user = new CurrentUser { Name = "Guest", Role = UserRoles.Guest };
            }

            var simpleLog = new SimpleLog
            {
                Controller = controllerName,
                Action = actionName,
                UserName = user.Name,
                UserRole = user.Role,
                Time = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString()
            };
            string jsonResult = JsonConvert.SerializeObject(simpleLog);
            new SshHelper().sendStringToFile("/home/linux/logs/simpleLog.log", jsonResult);
        }
    }
}
