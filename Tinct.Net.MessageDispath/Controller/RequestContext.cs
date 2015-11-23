using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tinct.Net.Message;
using Tinct.Net.MessageDispath.Route;

namespace Tinct.Net.MessageDispath.Controller
{
    public class RequestContext
    {
        public virtual RouteData RouteData { get; set; }

        public virtual DispathMessage  RemainDispathUrlMessage{ get; set; }

    }
}
