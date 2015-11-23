using System;
using System.Collections.Generic;
using Tinct.TinctTaskMangement.TinctWork;

namespace Tinct.TinctTaskMangement.Interface
{
    public interface ITinctTaskRepository
    {
       

        void AddTinctTask(TinctTask task);

        void RemoveTinctTask(TinctTask task);

        void UpdateTinctTasksStatus(string message);

        TinctTask GetTinctTaskByID(Guid ID);

    }
}
