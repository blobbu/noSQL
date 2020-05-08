using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using noSQL.Models;
using noSQL.Common;
using System.Web;
using Microsoft.AspNetCore.Session;

namespace noSQL.Controllers
{
    public class AccountController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        public void SubmitLogin(AccountModel model)
        {
            redisSetKey("klucz", "wartosc123qwe");
            if (ValidateUser(model.Login, model.Password))
            {
                //User validated
            }
            else
            {
                //Uset not validated
            }
        }

        public IActionResult Register()
        {
            return View();
        }
        public IActionResult SubmitRegister(AccountModel model)
        {
            MongoDatabase mongoDb = new MongoDatabase("mongodb://root:root@13.82.22.244:27017");
            mongoDb.setDatabase("noSQL");
            mongoDb.setCollection("users");
            ReturnInfo result = mongoDb.CreateNewUser(model.Login, model.Password);

            if (result.Status)
            {
                return View("Login");
            }
            else if(result.Message == "User exist")
            {
                ViewBag.Message = "Taki użytkownik już istnieje!!!";
                return View("Register");
            }
            else
            {
                return View("Error");
            }

        }

        private bool ValidateUser(string login, string password)
        {
            MongoDatabase mongoDb = new MongoDatabase("mongodb://root:root@13.82.22.244:27017");
            mongoDb.setDatabase("noSQL");
            mongoDb.setCollection("users");
            var filter = mongoDb.getFilterForValidation(login, "anorhb433");
            var user = mongoDb.getFilteredDocuments(filter);

            return user.Count == 1;
        }
    }
}