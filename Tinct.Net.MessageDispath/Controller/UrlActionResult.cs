using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Message.Machine;
using Tinct.Net.Message;

namespace Tinct.Net.MessageDispath.Controller
{
    public class UrlActionResult : ActionResult
    {

        public string RemainTaskData { get; set; }


        public override void ExecuteResult(ControllerContext context)
        {
            DispathMessage returnMessage = new DispathMessage();
            returnMessage.ControllerName = context.RequestContext.RouteData.Controller;
            returnMessage.ActionName = context.RequestContext.RouteData.ActionName;
            returnMessage.TaskID = context.RequestContext.RouteData.TaskID;
            returnMessage.TaskData = RemainTaskData;
            if (string.IsNullOrEmpty(RemainTaskData))
            {
                returnMessage.MachineInvokeStatus = MachineInvokeStatus.Completed;
            }
            else
            {
                returnMessage.MachineInvokeStatus = MachineInvokeStatus.PartCompleted;
            }

           
            context.RequestContext.RemainDispathUrlMessage = returnMessage;
        }
    }
}
