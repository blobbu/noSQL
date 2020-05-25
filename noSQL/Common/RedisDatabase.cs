using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace noSQL.Common
{
    public class RedisDatabase : DatabaseBase
    {
        private readonly ConnectionMultiplexer redisConn;
        public RedisDatabase()
        {
            ConfigurationOptions option = new ConfigurationOptions
            {
                EndPoints = { "192.168.8.101:6379" },
                Password = "redis"
            };

            redisConn = ConnectionMultiplexer.Connect(option);
        }

        public bool SetKey (string key, string value, int dbNumber = 1)
        {
            IDatabase conn = redisConn.GetDatabase(dbNumber);
            
            return conn.StringSet(key, value);
        }
        public bool SetKeyWithExpire(string key, string value, int seconds, int dbNumber = 1)
        {
            IDatabase conn = redisConn.GetDatabase(dbNumber);
            TimeSpan expireTime = new TimeSpan(0, 0, seconds);
            return conn.StringSet(key, value, expireTime);
        }

        public string GetValue(string key, int dbNumber = 1)
        {
            IDatabase conn = redisConn.GetDatabase(dbNumber);
            return conn.StringGet(key);
        }

        public bool DeleteKey(string key, int dbNumber = 1)
        {
            IDatabase conn = redisConn.GetDatabase(dbNumber);
            return conn.KeyDelete(key);
        }

    }
}
