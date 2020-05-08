using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cassandra;

namespace noSQL.Common
{
    public class CassandraDatabase : DatabaseBase
    {
        public CassandraDatabase()
        {
            Cluster cluster = Cluster.Builder()
                         .AddContactPoints("13.82.22.244")
                         .WithPort(9042)
                         .WithLoadBalancingPolicy(new DCAwareRoundRobinPolicy("datacenter1"))
                         .WithAuthProvider(new PlainTextAuthProvider("cassandra", "cassandra"))
                         .Build();
            var session = cluster.Connect("test_cassandra");
            var nazwa = cluster.Metadata.ClusterName;
            var rs = session.Execute("SELECT * FROM repair_history");
        }

    }
}
