﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tinct.Net.Communication.Connect
{
    public class TinctConnectReceiveArgs:EventArgs
    {
        public string ReceivedMessage { get; set; }

    }
}
