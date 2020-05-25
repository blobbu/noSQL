using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cassandra;

namespace noSQL.Common
{
    public class CassandraDatabase : DatabaseBase
    {
        public Cluster Clust { get; private set; }
        public CassandraDatabase()
        {
            Clust = Cluster.Builder()
                         .AddContactPoints("192.168.8.101")
                         .WithPort(9042)
                         .WithLoadBalancingPolicy(new DCAwareRoundRobinPolicy("datacenter1"))
                         .WithAuthProvider(new PlainTextAuthProvider("cassandra", "cassandra"))
                         .Build();
        }
        public RowSet getAllDataFromTable(string tableName)
        {
            var session = Clust.Connect("test_cassandra");
            var nazwa = Clust.Metadata.ClusterName;
            var rs = session.Execute("SELECT * FROM " + tableName);
            return rs;
        }

    }
}
