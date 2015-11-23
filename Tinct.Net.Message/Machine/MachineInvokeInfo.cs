using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tinct.Net.Message.Machine
{
    public class MachineInvokeInfo
    {


        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public Guid TaskID { get; set; }

        public MachineInvokeStatus Status { get; set; }

        public MachineInvokeInfo() { }

        public MachineInvokeInfo(Guid taskID, string controllerName,string actionName, MachineInvokeStatus status)
        {
            TaskID = taskID;
            ControllerName = controllerName;
            ActionName = actionName;
            Status = status;
        }
    }




}
