using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.MessageDispath.Controller;

namespace Tinct.PlatformController
{
    public  class TinctTestController: ControllerBase
    {

        public ActionResult LoadData(string taskDatas)
        {
            UrlActionResult result = new UrlActionResult();
       
            System.Threading.Thread.Sleep(20000);
            result.RemainTaskData = "";
            return result;
        }
    }
}
