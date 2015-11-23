using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Communication.Slave;
using Tinct.PlatformController;

namespace Tinct.Net.SlavePointConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            ///in order to load assembly
            TinctTestController t = new TinctTestController();
            TinctSlavePoint p = new TinctSlavePoint();
            p.StartSlave();
            Console.Read();
        }
    }
}
