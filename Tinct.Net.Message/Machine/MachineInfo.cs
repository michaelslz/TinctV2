using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Tinct.Net.Message.Machine
{
    public class MachineInfo
    {
        private List<MachineInvokeInfo> machineInvokeInfos = new List<MachineInvokeInfo>();
        public string MachineName { get; set; }

        public List<MachineInvokeInfo> MachineInvokeInfos
        {
            get
            {
                return machineInvokeInfos;
            }
            set
            {
                machineInvokeInfos = value;
            }
        }

        public string ToJsonSerializeString()
        {
            JsonSerializer serializer = new JsonSerializer();
            StringWriter sw = new StringWriter();
            serializer.Serialize(sw, this);


            return sw.GetStringBuilder().ToString();
        }

        public static MachineInfo GetObjectBySerializeString(string serializeString)
        {
            MachineInfo obj = null;
            JsonSerializer serializer = new JsonSerializer();
            StringReader reader = new StringReader(serializeString);
            try
            {
                obj = serializer.Deserialize<MachineInfo>(new JsonTextReader(reader));
            }
            catch
            {
                throw;
            }
            return obj;
        }
    }
}
