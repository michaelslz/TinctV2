using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Message;
using Tinct.Net.MessageDispath.Controller;
using Tinct.Net.MessageDispath.Controller.InterFace;
using Tinct.Net.MessageDispath.Route;
using Tinct.Net.MessageDispath.Route.Interface;

namespace Tinct.Net.MessageDispath.Route
{
    public  class UrlRouteHandler:IRouteHandler
    {


        public DispathMessage MapToControllerExcute(RouteData data)
        {
            RequestContext re = new RequestContext();
            re.RouteData = data;
            string controllerName = data.Controller;
            IControllerFactory controllerFactory = ControllerBuilder.Current.GetControllerFactory();
            IController controller = controllerFactory.CreateController(re, controllerName);
            controller.Execute(re);
            return re.RemainDispathUrlMessage;
        }
    }
}
