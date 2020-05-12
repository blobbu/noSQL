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
using noSQL.Helpers;

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
        public IActionResult SubmitLogin(AccountModel model)
        {
            
            if (ValidateUser(model.Login, model.Password))
            {
                int sessionExpireSeconds = 30 * 60; //sesja wygasa po 30 minutach
                UserRoles role = GetUserRole(model.Login);
                string redisValue = model.Login + ";" + role;
                redisSetKeyWithExpire(HttpContext.Session.Id, redisValue, sessionExpireSeconds);
                byte[] tmp = Encoding.ASCII.GetBytes("x");
                this.HttpContext.Session.Set("tmp", tmp);
                ViewBag.Message = "Zalogowano pomyślnie";
                return RedirectToAction("News", "Home");
            }
            else
            {
                ViewBag.Message = "Nieprawidłowe dane!!!";
                return View("Login");
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
            ReturnInfo result = mongoDb.CreateNewUser(model.Login, model.Password, UserRoles.User);

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
            var filter = mongoDb.getFilterForValidation(login, password);
            var user = mongoDb.getFilteredDocuments(filter);

            return user.Count == 1;
        }

        private UserRoles GetUserRole(string login)
        {
            MongoDatabase mongoDb = new MongoDatabase("mongodb://root:root@13.82.22.244:27017");
            mongoDb.setDatabase("noSQL");
            mongoDb.setCollection("users");
            var filter = mongoDb.getSimpleFilter("login", login);
            var user = mongoDb.getFilteredDocuments(filter);
            var role = mongoDb.GetUserRole(user[0]);

            if (role == 1) return UserRoles.User;
            else if (role == 2) return UserRoles.Admin;
            else return UserRoles.Guest;
        }

        
    }
}