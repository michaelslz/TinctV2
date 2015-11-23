using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Communication.Connect;
using Tinct.Net.Communication.Interface;
using Tinct.Net.Message.Machine;
using Tinct.Net.MessageDispath.Route;
using System.Configuration;



namespace Tinct.Net.Communication.Slave
{
    public class TinctSlavePoint : SlavePoint
    {

        private int masterPort = int.Parse(ConfigurationManager.AppSettings["MasterPort"].ToString());
        private int slavePort = int.Parse(ConfigurationManager.AppSettings["SlavePort"].ToString());
        private string masterName = ConfigurationManager.AppSettings["Master"].ToString();
        private string slaveName = ConfigurationManager.AppSettings["Slave"].ToString();

        private MachineInfo machineInfo = new MachineInfo();

        private object syncMachinestatus = new object();

        private Route route { get; set; }

        public TinctSlavePoint()
        {
            route = new Route();
            machineInfo.MachineName = slaveName;
        }


        public void MapToHandlerByMessage(object sender, TinctConnectReceiveArgs args)
        {
            var RoutData = route.GetRouteData(args.ReceivedMessage);
            var controllerName = RoutData.Controller;
            var actionName = RoutData.ActionName;
            var taskID = RoutData.TaskID;
            lock (syncMachinestatus)
            {

                machineInfo.MachineInvokeInfos.Add(new MachineInvokeInfo(taskID, controllerName, actionName, MachineInvokeStatus.Running));
            }

            new Task(() =>
            {
                var remainMessage = route.RouteHandler.MapToControllerExcute(RoutData);
                remainMessage.MachineName = slaveName;
                SendMessageToMaster(remainMessage.ToJsonSerializeString(), masterName);

                lock (syncMachinestatus)
                {

                    var macinfos = machineInfo.MachineInvokeInfos.Where(t => t.ControllerName == remainMessage.ControllerName && t.ActionName == remainMessage.ActionName && t.TaskID == remainMessage.TaskID).First();
                    macinfos.Status = remainMessage.MachineInvokeStatus;
                    if (macinfos.Status == MachineInvokeStatus.Completed
                              || macinfos.Status == MachineInvokeStatus.Exception
                              || macinfos.Status == MachineInvokeStatus.Fault
                              || macinfos.Status == MachineInvokeStatus.PartCompleted)
                    {
                        machineInfo.MachineInvokeInfos.Remove(macinfos);
                    }
                }

            }).Start();


        }



        public TinctSlavePoint(ITinctConnect tinctCon)
        {
            this.tinctCon = tinctCon;
        }

        public void SendMessageToMaster(string message, string machineName)
        {

            tinctCon.SendMessage(machineName, masterPort, message, false);

        }


        public override bool StartSlave()
        {

            tinctCon = new TinctConnect();
            tinctCon.AfterReceiveMessage += new EventHandler<TinctConnectReceiveArgs>(MapToHandlerByMessage);



            System.Threading.Timer t = new System.Threading.Timer
                (
                    (con) =>
                    {
                        string message = "";
                        lock (syncMachinestatus)
                        {
                            message = machineInfo.ToJsonSerializeString();
                        }

                     
                        lock (syncMachinestatus)
                        {

                            int count = machineInfo.MachineInvokeInfos.Count;
                            for (int i = 0; i < count; i++)
                            {
                                if (machineInfo.MachineInvokeInfos[i].Status == MachineInvokeStatus.Completed
                                || machineInfo.MachineInvokeInfos[i].Status == MachineInvokeStatus.Exception
                                || machineInfo.MachineInvokeInfos[i].Status == MachineInvokeStatus.Fault
                                || machineInfo.MachineInvokeInfos[i].Status == MachineInvokeStatus.PartCompleted)
                                {
                                    machineInfo.MachineInvokeInfos.RemoveAt(i);
                                }
                            }

                        }
                        (con as TinctConnect).SendMessage(masterName, masterPort, message, false);

                    },
                    tinctCon, 1000, 10000
                );

            int port = 0;
            int.TryParse(ConfigurationManager.AppSettings["SlavePort"], out port);
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
    }
}
