using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.TinctTaskMangement.Interface;
using Tinct.TinctTaskMangement.TinctWork;

namespace Tinct.TinctTaskMangement.Base
{
     public abstract  class BaseTinctTaskRepository: ITinctTaskRepository
    {


        public virtual int AvailableTinctTasksCount { get; set; }

        public virtual TinctTask AvailableTinctTask { get; set; }


        protected virtual Queue<TinctTask> AvailableTinctTasks { get; set; }
        public virtual event EventHandler<EventArgs> EnqueTask;
        public abstract void AddTinctTask(TinctTask task);


        public abstract TinctTask GetTinctTaskByID(Guid ID);


        public abstract void RemoveTinctTask(TinctTask task);


        public abstract void UpdateTinctTasksStatus(string message);
       
    }
}
