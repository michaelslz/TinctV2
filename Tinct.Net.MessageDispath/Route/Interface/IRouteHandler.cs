using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Message;
using Tinct.Net.MessageDispath.Route;

namespace Tinct.Net.MessageDispath.Route.Interface
{
    public interface IRouteHandler
    {
        DispathMessage MapToControllerExcute(RouteData data);
    }
}
