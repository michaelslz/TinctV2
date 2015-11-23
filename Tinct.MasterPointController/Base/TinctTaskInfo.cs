using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Tinct.TinctTaskMangement.Base
{
    public class TinctTaskInfo
    {
        [JsonIgnore]
        private List<Guid> waitTaskIDs = new List<Guid>();
        public TinctTaskInfo(TinctTaskContext context, TinctTaskStatus taskStatus)
        {
            Context = context;
            Status = taskStatus;
        }

        public TinctTaskContext Context{ get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public TinctTaskStatus Status { get; set; }

        public List<Guid> WaitTaskIDs { get { return waitTaskIDs; } set { waitTaskIDs = value; } }

    }
}
