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

        public virtual void EndMaster()
        {
          
        }




        public virtual void StartMaster()
        {
            
        }
    }
}
