using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace noSQL.Helpers
{
    public class SshHelper
    {
        public bool sendStringToFile(string filePath, string stringData)
        {
            using (var client = new SshClient("13.82.22.244", "linux", "qscvb1234ZKLIFG"))
            {
                client.Connect();
                client.RunCommand("touch " + filePath);
                client.RunCommand("echo '" + stringData + "' >> " + filePath);
                client.Disconnect();
            }
            return true;
        }
    }
}
