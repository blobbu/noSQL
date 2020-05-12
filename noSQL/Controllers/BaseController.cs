using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using noSQL.Common;
using noSQL.Helpers;

namespace noSQL.Controllers
{
    public class BaseController : Controller
    {
        private readonly RedisDatabase redis = new RedisDatabase();
        public CurrentUser CurrentUser
        {
            get
            {
                return getCurrentUser();
            }
        }

        protected bool redisSetKey(string value, string key, int dbNumber = 1)
        {
            return redis.SetKey(value, key, dbNumber);
        }
        protected bool redisSetKeyWithExpire(string value, string key, int expireTime, int dbNumber = 1)
        {
            return redis.SetKeyWithExpire(value, key, expireTime, dbNumber);
        }
        private string redisGetValue(string key, int dbNumber = 1)
        {
            return redis.GetValue(key);
        }
        private CurrentUser getCurrentUser()
        {
            UserRoles role = UserRoles.Guest;

            string rawRedis = redisGetValue(this.HttpContext.Session.Id);
            string name = "";
            try
            {
                string[] redisInfo = rawRedis.Split(';');
                name = redisInfo[0];
                if (redisInfo[1] == "User") role = UserRoles.User;
                else if (redisInfo[1] == "Admin") role = UserRoles.Admin;
            }
            catch
            {
                new CurrentUser { Name = name, Role = role };
            }

            return new CurrentUser{ Name = name, Role = role };
        }
        protected bool isLoggedIn()
        {
            if (CurrentUser != null)
                return true;
            else
                return false;
        }
    }
}