using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.TinctTaskMangement.Interface;

namespace Tinct.TinctTaskMangement.Base
{
    public abstract class BaseTinctTask : ITinctTask
    {
        public Guid ID
        {
            get;
            protected set;
        }

        public string Name { get; set; }

        public bool IsCancel { get; protected set; }
        public virtual TinctTaskContext Context
        {
            get; set;
        }


        public virtual TinctTaskStatus TaskStatus { get; set; }

        public virtual void Start() { }
        public virtual void Start(Action action) { }

        public abstract void Start(Action<TinctTaskContext> action);
        public abstract void Cancel();

        public abstract void Wait();

        public abstract void Wait(int millsencond);

        public abstract void Dispose();

  
      

      

    }
}
