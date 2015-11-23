using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Communication.Connect;
using Tinct.Net.Communication.Interface;

namespace Tinct.Net.Communication.Slave
{
    public class SlavePoint:ISlaveConnect
    {
        public ITinctConnect tinctCon=null;
        public virtual bool StartSlave()
        {
            tinctCon = new TinctConnect();
            int port = 0;
            int.TryParse(ConfigurationManager.AppSettings["Port"], out port);
            if (port == 0)
            {
                return false;
            }
            if (tinctCon.StartSlaveServer(port))
            {
                Console.WriteLine("Start Slave Complete!");
                return true;

            }
            else { return false; }
         
        }


        public virtual void   EndSlave()
        {
            if (tinctCon != null) 
            {
                tinctCon.EndServer();
            }
        }
    }
}
