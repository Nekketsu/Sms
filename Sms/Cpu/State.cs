using System;
using System.Collections.Generic;
using System.Text;

namespace Sms.Cpu
{
    public class State
    {
        public bool Halted { get; set; }
        public bool NMIServicing { get; set; }
        public int InterruptMode { get; set; }
    }
}
