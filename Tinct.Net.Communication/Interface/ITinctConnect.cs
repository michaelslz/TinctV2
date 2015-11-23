using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Communication.Connect;

namespace Tinct.Net.Communication.Interface
{
    public interface ITinctConnect
    {
         bool Connect(string machineName, int port);

         event EventHandler<TinctConnectReceiveArgs> AfterReceiveMessage;
         event EventHandler<TinctConnectReceiveArgs> ReceiveRestMessage;
         void SendMessage(string machineName, int port, string message, bool checkconnect);

         bool StartSlaveServer(int port);

         bool StartMasterServer(int port);
         void EndServer();
     
 
    }
}
