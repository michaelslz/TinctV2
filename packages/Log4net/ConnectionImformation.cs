using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGT.DBOperation
{
    public class ConnectionImformation
    {
        public string Featureconstr { get; set; }

    }


    public enum ConnectionStatus
    {
     
        NoUse,
        InUse
    }

}
