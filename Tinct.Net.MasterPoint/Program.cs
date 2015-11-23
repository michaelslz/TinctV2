using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Message;
using Tinct.TinctTaskMangement;
using Tinct.TinctTaskMangement.Base;
using Tinct.TinctTaskMangement.Interface;
using Tinct.TinctTaskMangement.TinctWork;

namespace Tinct.Net.MasterPointConsole
{
    class Program
    {
        static void Main(string[] args)
        {
         

            TinctTask t1 = new TinctTask();
            t1.TaskStatus = TinctTaskStatus.WaittingToRun;
            t1.Context.TaskData = "";
            t1.Context.ControllerName = "TinctTest";
            t1.Context.ActionName = "LoadData";
            t1.Context.TaskID = t1.ID;

            TinctTask t = new TinctTask();
            t.TaskStatus = TinctTaskStatus.Waitting;
            t.Context.TaskData = "";
            t.Context.ControllerName = "TinctTest";
            t.Context.ActionName = "LoadData";
            t.Context.TaskID = t.ID;
            t.WaittingTinctTasks.Add(t1);

            TinctTask t2 = new TinctTask();
            t2.TaskStatus = TinctTaskStatus.WaittingToRun;
            t2.Context.TaskData = "";
            t2.Context.ControllerName = "TinctTest";
            t2.Context.ActionName = "LoadData";
            t2.Context.TaskID = t2.ID;


            BaseTinctTaskRepository tre = new TinctTaskRepository();
            tre.AddTinctTask(t);
            tre.AddTinctTask(t1);
            tre.AddTinctTask(t2);
            TinctTaskMangement.TinctWork.TinctTaskManeger tm = new TinctTaskMangement.TinctWork.TinctTaskManeger();

            tm.Repository = tre;
            tm.Start();

            Console.Read();


        }
    }
}
