using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tinct.Net.Communication.Connect;
using Tinct.Net.Communication.Interface;
using Tinct.Net.Message.Machine;
using System.Configuration;
using System.IO;

namespace Tinct.Net.Communication.Master
{
    public class TinctMasterPoint : MasterPoint
    {
        private object syncMachineInfoObject = new object();

        private int masterPort = int.Parse(ConfigurationManager.AppSettings["MasterPort"]);

        private List<MachineInfo> machineInfolist = new List<MachineInfo>();

        public List<MachineInfo> MachineInfolist
        {

            get
            {
                return MachineInfolist;
            }
            private set { }
        }


        public TinctMasterPoint()
        {
            tinctCon = new TinctConnect();
        }

        public TinctMasterPoint(ITinctConnect tinctCon)
        {
            this.tinctCon = tinctCon;
        }

        #region Statusmange

        public void UpdateMachineInfo(string message)
        {

            MachineInfo machineInfo = MachineInfo.GetObjectBySerializeString(message);
            UpdateMachineInfo(machineInfo);
        }

        public void UpdateMachineInfo(MachineInfo machineInfo)
        {

            lock (syncMachineInfoObject)
            {
                int count = machineInfolist.Count;
                bool searchmac = false;
                for (int i = 0; i < count; i++)
                {
                    if (machineInfolist[i].MachineName == machineInfo.MachineName)
                    {
                        searchmac = true;
                        machineInfolist[i].MachineInvokeInfos = machineInfo.MachineInvokeInfos;
                        int mivcount = machineInfo.MachineInvokeInfos.Count;
                        for (int j = 0; j < mivcount; j++)
                        {
                            ///TO DO....
                            if (machineInfo.MachineInvokeInfos[j].Status == MachineInvokeStatus.Completed)
                            {
                                ///log 
                                string logmessage = machineInfo.MachineName + " " + machineInfo.MachineInvokeInfos[j].TaskID + " " + machineInfo.MachineInvokeInfos[j].ActionName + " " +
                                    machineInfo.MachineInvokeInfos[j].ControllerName + " " + machineInfo.MachineInvokeInfos[j].Status + "\r\n";
                                File.AppendAllText(Environment.CurrentDirectory + @"\\Task.txt", logmessage);
                            }
                        }

                    }

                }
                if (!searchmac)
                {
                    machineInfolist.Add(machineInfo);
                }

            }

        }

        public string GetAvaiableMachineName(string controllerName, string actionName)
        {
            List<string> avalibleMachines = new List<string>();
            lock (syncMachineInfoObject)
            {
                foreach (var m in machineInfolist)
                {
                    var count = m.MachineInvokeInfos.
                    Where(ms => ms.ControllerName == controllerName
                    && ms.ActionName == actionName
                    &&ms.Status==MachineInvokeStatus.Running).Count();
                    if (count == 0)
                    {
                        avalibleMachines.Add(m.MachineName);
                    }
                }
            }
            int avalibleCount = avalibleMachines.Count;
            if (avalibleCount == 0)
            {
                return "";
            }

            Random ran = new Random();
            var index= ran.Next(0, avalibleCount);

            return avalibleMachines[index];


        }

        #endregion


        public void SendMessageToSlave(string message, string machineName)
        {

            int port = int.Parse(ConfigurationManager.AppSettings["SlavePort"].ToString());

            tinctCon.SendMessage(machineName, port, message, false);

        }


        public override void StartMaster()
        {
            tinctCon.StartMasterServer(masterPort);

            Timer t = new Timer((obj) => 
            {

            },);


        }

        public override void EndMaster()
        {
            MachineInfolist.Clear();
        }


    }
}
