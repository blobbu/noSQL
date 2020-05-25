using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Nest;
using noSQL.Models;
namespace noSQL.Common
{
    public class Elastic
    {
        public ElasticClient EsClient()
        {
            var nodes = new Uri[]
            {
                new Uri("http://192.168.8.101:5601/"),
            };

            var connectionPool = new StaticConnectionPool(nodes);
            var connectionSettings = new ConnectionSettings(connectionPool).DisableDirectStreaming();

            var elasticClient = new ElasticClient(connectionSettings);
            return elasticClient;
        }

    }
}
