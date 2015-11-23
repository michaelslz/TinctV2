using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tinct.TinctTaskMangement.Interface;
using Tinct.Net.Communication.Master;
using Tinct.Net.Communication.Connect;
using Tinct.TinctTaskMangement.Base;

namespace Tinct.TinctTaskMangement.TinctWork
{
    public class TinctTask:BaseTinctTask
    {
       
        private ManualResetEvent syncWaitObject = new ManualResetEvent(false);
        private List<TinctTask> waittingTinctTasks = new List<TinctTask>();
        private TinctTaskStatus taskStatus = TinctTaskStatus.WaittingToRun;

        
        public override TinctTaskStatus TaskStatus
        {
            get { return taskStatus; }
            set
            {
                TinctTaskStatus oriTaskStatus = taskStatus;
                taskStatus = value;
                if (taskStatus == TinctTaskStatus.Completed || taskStatus == TinctTaskStatus.Completed)
                {
                    syncWaitObject.Set();
                }
                if (TaskStatus == TinctTaskStatus.Waitting)
                {
                    new Task(() =>
                    {
                        foreach (var waitTask in WaittingTinctTasks)
                        {
                            waitTask.Wait();
                        }
                        TaskStatus = TinctTaskStatus.WaittingToRun;
                    }).Start(); ;
                    
                }
                if (TinctTaskStatusChanged != null && oriTaskStatus != taskStatus)
                {
                    TinctTaskEventArgs e = new TinctTaskEventArgs();
                    e.Context = Context;
                    TinctTaskStatusChanged(this, e);
                }

            }
        }
        public List<TinctTask> WaittingTinctTasks
        {
            get
            {
                return waittingTinctTasks;
            }
            set
            {
                waittingTinctTasks = value;
            }
        }

        public event EventHandler<TinctTaskEventArgs> TinctTaskCompleted;
        public event EventHandler<TinctTaskEventArgs> TinctTaskStatusChanged;
      
        public TinctTask()
        {
            ID = Guid.NewGuid();
            WaittingTinctTasks = new List<TinctTask>();
            Context = new TinctTaskContext();
        }
        public TinctTask(String name)
        {
            Name = name;
            WaittingTinctTasks = new List<TinctTask>();
        }

        public override void Start(Action<TinctTaskContext> action)
        {
           
       
            if (action != null)
            {
                action(Context);
            }
            TaskStatus = TinctTaskStatus.Running;
        }

        public override void Cancel()
        {
            //to do log it
            IsCancel = true;
            syncWaitObject.Set();
        }

        public override void Wait()
        {
            syncWaitObject.WaitOne();
            if (!IsCancel)
            {
                TaskStatus = TinctTaskStatus.Completed;
            }
            else
            {
                TaskStatus = TinctTaskStatus.Canceled;
            }

        }

        public override void Wait(int millsecond)
        {
            if (syncWaitObject.WaitOne(millsecond))
            {
                TaskStatus = TinctTaskStatus.Completed;
            }
            else
            {
                TaskStatus = TinctTaskStatus.Canceled;
            }
       
        }

        public override void Dispose()
        {
            syncWaitObject.Dispose();
        }


    }
}
