using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.TinctTaskMangement.Interface;
using Tinct.Net.Communication.Master;
using System.Threading;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;
using Tinct.Net.Message.Machine;
using Tinct.TinctTaskMangement.Base;

namespace Tinct.TinctTaskMangement.TinctWork
{
    public class TinctTaskManeger : ITinctTaskManeger
    {
        EventWaitHandle singal = new EventWaitHandle(false, EventResetMode.ManualReset, "tasksingal");
        EventWaitHandle macsingal = new EventWaitHandle(false, EventResetMode.ManualReset, "macsingal");
        private TinctMasterPoint msp = new TinctMasterPoint();

        private bool updateMachinesingal = false;

        private string currentControllerName = "";
        private string currentActionName = "";
        private string currentMachinename = "";
        private int masterPort = int.Parse(ConfigurationManager.AppSettings["MasterPort"]);

        public TinctTaskManeger()
        {
            msp.tinctCon.ReceiveRestMessage += TinctCon_ReceiveRestMessage;
        }

        public void Start()
        {
            if (Repository == null)
            {
                Console.WriteLine("There is no TinctTask,Please arrange task! ");
                return;
            }

            Task t = new Task
            (
                () => { msp.tinctCon.StartMasterServer(masterPort); }
            );
            t.Start();

            Repository.EnqueTask += Repository_EnqueTask;
            while (true)
            {
                BaseTinctTask task = null;
                if (Repository.AvailableTinctTasksCount == 0)
                {
                    singal.WaitOne();
                    singal.Reset();
                }
                else
                {
                    task = Repository.AvailableTinctTask;
                }
                if (task == null)
                {
                    return;
                }



                task.Start((context) =>
                {
                    currentMachinename = msp.GetAvaiableMachineName(context.ControllerName, context.ActionName);
                    if (string.IsNullOrEmpty(currentMachinename))
                    {

                        updateMachinesingal = true;
                        currentControllerName = context.ControllerName;
                        currentActionName = context.ActionName;
                        macsingal.WaitOne();
                        macsingal.Reset();
                        updateMachinesingal = false;
                    }
                    context.MachineName = currentMachinename;
                    JsonSerializer serializer = new JsonSerializer();
                    using (StringWriter sw = new StringWriter())
                    {
                        serializer.Serialize(sw, context);
                        string message = sw.GetStringBuilder().ToString();

                        ///to do rewrite message

                        new Task(() => { msp.SendMessageToSlave(message, currentMachinename); }).Start();

                        MachineInfo machineInfo = new MachineInfo();
                        machineInfo.MachineName = currentMachinename;
                        machineInfo.MachineInvokeInfos.Add(new MachineInvokeInfo(task.ID, currentControllerName, currentActionName, MachineInvokeStatus.Running));
                        msp.UpdateMachineInfo(machineInfo);
                    }

                });


            }

        }

        private void Repository_EnqueTask(object sender, EventArgs e)
        {


            singal.Set();

        }

        private void TinctCon_ReceiveRestMessage(object sender, Net.Communication.Connect.TinctConnectReceiveArgs e)
        {
            ///update machine status
            if (e.ReceivedMessage.StartsWith("{\"MachineName\""))
            {
                msp.UpdateMachineInfo(e.ReceivedMessage);
                if (updateMachinesingal)
                {
                    currentMachinename = msp.GetAvaiableMachineName(currentControllerName, currentActionName);
                    if (!string.IsNullOrEmpty(currentMachinename))
                    {
                        macsingal.Set();
                    }

                }
            }
            //update task status
            if (e.ReceivedMessage.StartsWith("{\"TaskID\""))
            {
                Repository.UpdateTinctTasksStatus(e.ReceivedMessage);

            }

        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public BaseTinctTaskRepository Repository { get; set; }
    }
}
