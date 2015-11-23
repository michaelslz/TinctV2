using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Message;
using Tinct.Net.MessageDispath.Controller.InterFace;

namespace Tinct.Net.MessageDispath.Controller
{
    public class ControllerActionInvoker : IActionInvoker
    {
        public ControllerActionInvoker()
        {
        }
        public void InvokeAction(ControllerContext controllerContext, string actionName)
        {
            MethodInfo method = controllerContext.Controller.GetType().GetMethod(actionName);
            List<object> parameters = new List<object>();
            object taskData;
            controllerContext.RequestContext.RouteData.Values.TryGetValue("taskData", out taskData);
       
            parameters.Add(taskData);
            ActionResult actionResult = method.Invoke(controllerContext.Controller,parameters.ToArray()) as ActionResult;
            actionResult.ExecuteResult(controllerContext);
        
        }
    }
}
