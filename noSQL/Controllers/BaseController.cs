using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using noSQL.Common;

namespace noSQL.Controllers
{
    public class BaseController : Controller
    {
        private readonly RedisDatabase redis = new RedisDatabase();

        protected bool redisSetKey(string value, string key, int dbNumber = 1)
        {
            return redis.SetKey(value, key, dbNumber);
        }
    }
}