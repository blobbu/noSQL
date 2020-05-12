using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using noSQL.Controllers;

namespace noSQL.Filters
{
    public class ViewBagAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var controller = filterContext.Controller as BaseController;
            controller.ViewBag.UserName = controller.CurrentUser.Name;
        }
    }
}
