using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerProgrammer.Common.Interfaces {
    public class ControllerRecipe {
        public double Led1Delay { get; set; }
        public double Led1RunTime { get; set; }
        public double Led1Current { get; set; }

        public double Led2Delay { get; set; }
        public double Led2RunTime { get; set; }
        public double Led2Current { get; set; }

        public double Led3Delay { get; set; }
        public double Led3RunTime { get; set; }
        public double Led3Current { get; set; }
    }

    public interface IControllerManager {
        bool Connect();
        bool Disconnect();
        bool FindSerialPower();
        bool ProgramController(ControllerRecipe recipe);
    }
}
