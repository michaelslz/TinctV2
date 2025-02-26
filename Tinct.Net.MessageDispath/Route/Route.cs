﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Message;
using Tinct.Net.MessageDispath.Controller;
using Tinct.Net.MessageDispath.Route.Interface;

namespace Tinct.Net.MessageDispath.Route
{
    public class Route : RouteBase
    {
        public IRouteHandler RouteHandler { get; set; }
        public Route()
        {
            this.RouteHandler = new UrlRouteHandler();
            
            Controller.ControllerBuilder.Current.SetControllerFactory(new DefaultControllerFactory());
            Controller.ControllerBuilder.Current.DefaultNamespaces = new HashSet<string>(new List<string>() { "Tinct.PlatformController" });
        }

        public override RouteData GetRouteData(string urlmessage)
        {
            DispathMessage obj;
            RouteData routeData = new RouteData();
            routeData.Route = this;
            try
            {
                obj = DispathMessage.GetObjectBySerializeString(urlmessage);
            }
            catch
            {
                throw;
            }
            routeData.Values.Add("controller", obj.ControllerName);
            routeData.Values.Add("action", obj.ActionName);
            routeData.Values.Add("taskID", obj.TaskID);
            routeData.Values.Add("taskData", obj.TaskData);
            routeData.RouteHandler = RouteHandler;
            return routeData;
        }


    }
}
