using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Message;
using Tinct.TinctTaskMangement.Interface;
using Tinct.Net.Message.Machine;
using Tinct.TinctTaskMangement.Base;
using Tinct.Common.Log;
using Newtonsoft.Json;
using System.IO;

namespace Tinct.TinctTaskMangement.TinctWork
{
    public class TinctTaskRepository : BaseTinctTaskRepository
    {
        private List<TinctTask> tinctTasks = new List<TinctTask>();

        private Queue<TinctTask> queuetasks = new Queue<TinctTask>();

        private ILogger logger = new LogManger().GetLogger();
        private LogEntity logentity = new LogEntity();


        public object syncqueue = new object();
        public object synctasks = new object();

        protected override  Queue<TinctTask> AvailableTinctTasks
        {
            get
            {
                lock (syncqueue)
                {
                    return queuetasks;
                }
            }
            
        }

        public override int AvailableTinctTasksCount
        {
            get
            {
                lock (syncqueue)
                {
                    return queuetasks.Count;
                }
            }

        }

        public override TinctTask AvailableTinctTask
        {
            get
            {
                lock (syncqueue)
                {
                    return queuetasks.Dequeue();
                }
            }


        }
        public override event EventHandler<EventArgs> EnqueTask;

        private void Task_TinctTaskStatusChanged(object sender, TinctTaskEventArgs e)
        {
            TinctTask task = sender as TinctTask;
            //log
            var taskInfo = new TinctTaskInfo(task.Context, task.TaskStatus);
            foreach (var w in task.WaittingTinctTasks)
            {
                taskInfo.WaitTaskIDs.Add(w.ID);
            }

            logentity.Message = taskInfo;
            logger.LogInfo(logentity);
         
            switch (task.TaskStatus)
            {

                case TinctTaskStatus.Completed:
                    break;
                case TinctTaskStatus.PartCompleted:
                    break;
                case TinctTaskStatus.WaittingToRun:
                    lock (syncqueue)
                    {
                        queuetasks.Enqueue(task);
                        if (EnqueTask != null)
                        {
                            EnqueTask(this,new EventArgs());
                        }
                        break;
                    }
                case TinctTaskStatus.Faulted:
                     break;
                case TinctTaskStatus.Running:
                    break;
                case TinctTaskStatus.Waitting:
                    break;
                case TinctTaskStatus.Canceled:                  
                    break;
                case TinctTaskStatus.Exception:
                    break;
                
            }
             
            
        }

        public override void AddTinctTask(TinctTask task)
        {
            task.TinctTaskStatusChanged += Task_TinctTaskStatusChanged;


            //log
            var taskInfo = new TinctTaskInfo(task.Context, task.TaskStatus);
            foreach (var w in task.WaittingTinctTasks)
            {
                taskInfo.WaitTaskIDs.Add(w.ID);
            }

            logentity.Message = taskInfo;
            logger.LogInfo(logentity);


            lock (synctasks)
            {
                tinctTasks.Add(task);
            }
            if (task.TaskStatus == TinctTaskStatus.WaittingToRun)
            {
                lock (syncqueue)
                {
                    queuetasks.Enqueue(task);
                    if (EnqueTask != null)
                    {
                        EnqueTask(this, new EventArgs());
                    }
                }
            }
        }

        public override TinctTask GetTinctTaskByID(Guid ID)
        {
            lock (synctasks)
            {
                return tinctTasks.Single(x => x.ID == ID);
            }
        }

        public override void RemoveTinctTask(TinctTask task)
        {
            lock (synctasks)
            {
                tinctTasks.Remove(task);
            }
        }

        public override void UpdateTinctTasksStatus(string message)
        {
            var dispathMessage=DispathMessage.GetObjectBySerializeString(message);
            var tinctTask = GetTinctTaskByID(dispathMessage.TaskID);

            switch (dispathMessage.MachineInvokeStatus)
            {
                       
                case MachineInvokeStatus.Completed:
                    tinctTask.TaskStatus = TinctTaskStatus.Completed;
                    break;
                case MachineInvokeStatus.Exception:
                    tinctTask.TaskStatus = TinctTaskStatus.Exception;
                    break;
                case MachineInvokeStatus.Fault:
                    tinctTask.TaskStatus = TinctTaskStatus.Faulted;
                    break;
                case MachineInvokeStatus.NoAction:break;
                case MachineInvokeStatus.PartCompleted:
                    tinctTask.TaskStatus = TinctTaskStatus.PartCompleted;
                    break;
                case MachineInvokeStatus.Running:
                    tinctTask.TaskStatus = TinctTaskStatus.Running;
                    break;

            }
        }
    }
}
