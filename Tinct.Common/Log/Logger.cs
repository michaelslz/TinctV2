using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = @"..\..\Log\Log4net.config", Watch = true)]
namespace Tinct.Common.Log
{
    public class Logger : ILogger
    {
        public void LogInfo(LogEntity logEntity)
        {
            ILog logger = LogManager.GetLogger("log4net");
            string message = "";
            JsonSerializer serializer = new JsonSerializer();
            using (StringWriter sw = new StringWriter())
            {
                serializer.Serialize(sw, logEntity);
                message = sw.GetStringBuilder().ToString();
            }
            logger.Info(message);
        }

        public void LogMessage(string message)
        {
            ILog logger = LogManager.GetLogger("log4net");
            logger.Info(message);
        }
    }
}
