using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.TinctTaskMangement.Base;

namespace Tinct.TinctTaskMangement.Interface
{
    public interface ITinctTask
    {
        void Start();
        void Start(Action action);
        void Start(Action<TinctTaskContext> action);
        void Cancel();
        void Wait();
        void Wait(int millsencond);

        void Dispose();


    }
}
