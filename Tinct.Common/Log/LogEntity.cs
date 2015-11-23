using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tinct.Common.Log
{
    public   class LogEntity
    {
        public DateTime Date { get { return DateTime.Now; } }

        public object Message { get; set; }

    }
}
