using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Communication.Interface;
using System.Net.Sockets;
using System.Net;
using Tinct.Net.Communication.Connect;
using System.Configuration;

namespace Tinct.Net.Communication.Master
{
    public  class MasterPoint : IMasterConnect
    {
        public ITinctConnect tinctCon = null;

        public virtual bool StartMaster(IList<string> slavenames)
        {
            tinctCon = new TinctConnect();
            int port = int.Parse(ConfigurationManager.AppSettings["SlavePort"].ToString());

            foreach(var slavename in slavenames)
            {
                if (tinctCon.Connect(slavename, port))
                {
                  
                    Console.WriteLine("Connect {0} successful",slavename);
                    return true;
                }
                else 
                {
                    Console.WriteLine("Connect {0} failed", slavename);
                }
            }
          






            return false;
        }

        

        public virtual void EndMaster()
        {
          
        }

       

    }
}
