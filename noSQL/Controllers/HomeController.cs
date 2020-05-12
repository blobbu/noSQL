using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using noSQL.Models;
using noSQL.Common;
using noSQL.Filters;

namespace noSQL.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult News()
        {
            return View();
        }

        [Authorize]
        public IActionResult VideoSearch()
        {
            //new Elastic().EsClient();
            return View();
        }

        public IActionResult LayoutUserName()
        {
            return View();
        }
    }
}
