using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.MessageDispath.Route.Interface;

namespace Tinct.Net.MessageDispath.Route
{
    public class RouteData
    {
        public IDictionary<string, object> Values { get; private set; }
        public IRouteHandler RouteHandler { get; set; }
        public RouteBase Route { get; set; }

        public RouteData() 
        {
            Values = new Dictionary<string, object>();
        }

        public string Controller
        {
            get
            {
                object controllerName = string.Empty;
                this.Values.TryGetValue("controller", out controllerName);
                return controllerName.ToString();
            }
        }
        public string ActionName
        {
            get
            {
                object actionName = string.Empty;
                this.Values.TryGetValue("action", out actionName);
                return actionName.ToString();
            }
        } 

        public Guid TaskID
        {
            get
            {
                object taskID = string.Empty;
                this.Values.TryGetValue("taskID", out taskID);
                return new Guid( taskID.ToString());
            }
        }
    }
}
