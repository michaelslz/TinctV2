
using System;
using Tinct.Net.Communication.Connect;
using Tinct.Net.Communication.Interface;

namespace Tinct.TinctTaskMangement.Base
{
    public class TinctTaskContext
    {
        public string MachineName { get; set; }

        public Guid TaskID { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }

        public string TaskData { get; set; }

    }
}