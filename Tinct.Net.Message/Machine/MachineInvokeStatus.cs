﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tinct.Net.Message.Machine
{
    public enum MachineInvokeStatus
    {
        NoAction,
        Running,
        Completed,
        PartCompleted,
        Fault,
        Exception

    }
}
