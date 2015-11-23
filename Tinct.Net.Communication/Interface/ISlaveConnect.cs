using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tinct.Net.Communication.Interface
{
    public  interface ISlaveConnect
    {

         bool StartSlave();
         void EndSlave();
    }
}
