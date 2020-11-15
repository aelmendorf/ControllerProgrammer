using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerProgrammer.Data.Model {

    public enum Wavelength {
        W285,
        W300,
        W310
    }

    //public class Recipe {
    //    public int Id { get; set; }

    //    public int BoardId { get; set; }

    //    public int CycleTime { get; set; }

    //    public int Led1Delay { get; set; }
    //    public int Led2Delay { get; set; }
    //    public int Led3Delay { get; set; }

    //    public int Led1RunTime { get; set; }
    //    public int Led2RunTime { get; set; }
    //    public int LEd3RunTime { get; set; }

    //}

    public class PowerDensity{
        public int Id { get; set; }
        public int LedId { get; set; }
        public Led Led { get; set; }
        public double Current { get; set; }
        public double PowerDenisty { get; set; }
    }

    public class Led {
        public int Id { get; set; }
        public Wavelength Wavelength { get; set; }
        public ICollection<PowerDensity> PowerDensities { get; set; }
        
    }
}
