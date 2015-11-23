using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tinct.TinctTaskMangement.Base
{
    public enum TinctTaskStatus
    {
        WaittingToRun,
        Waitting,
        Running,
        Completed,
        PartCompleted,
        Canceled,
        Faulted,
        Exception

    }
}
