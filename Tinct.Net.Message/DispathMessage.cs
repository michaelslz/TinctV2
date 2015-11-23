using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.IO;
using Tinct.Net.Message.Machine;

namespace Tinct.Net.Message
{



    public class DispathMessage
    {
        public Guid TaskID { get; set; }

        public string ControllerName { get; set; }
        public string ActionName { get; set; }


        public string MachineName { get; set; }

        public MachineInvokeStatus MachineInvokeStatus { get; set; }

     

        public string TaskData { get; set; }


        public string ToJsonSerializeString()
        {
            JsonSerializer serializer = new JsonSerializer();
            using (StringWriter sw = new StringWriter())
            {
                serializer.Serialize(sw, this);
                return sw.GetStringBuilder().ToString();
            }
        }


        public static DispathMessage GetObjectBySerializeString(string serializeString)
        {
            DispathMessage obj = null;
            JsonSerializer serializer = new JsonSerializer();
            using (StringReader reader = new StringReader(serializeString))
            {
                try
                {
                    obj = serializer.Deserialize<DispathMessage>(new JsonTextReader(reader));
                }
                catch
                {
                    throw;
                }
                return obj;
            }
        }

    }
}
