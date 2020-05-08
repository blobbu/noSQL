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
                //adres ip servera
                EndPoints = { "192.168.120.100:6379" }
            };

            redisConn = ConnectionMultiplexer.Connect(option);
            IDatabase conn = redisConn.GetDatabase(1);
            conn.StringSet("foo", "bar");
        }

        public bool SetKey (string key, string value, int dbNumber = 1)
        {
            IDatabase conn = redisConn.GetDatabase(dbNumber);
            
            return conn.StringSet(key, value);
        }

        public string GetValue(string key, int dbNumber = 1)
        {
            IDatabase conn = redisConn.GetDatabase(dbNumber);
            return conn.StringGet(key);
        }

    }
}
