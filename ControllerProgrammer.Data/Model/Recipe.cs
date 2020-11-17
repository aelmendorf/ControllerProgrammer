using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerProgrammer.Data.Model {

    public enum Wavelength {
        W285,
        W300,
        W310
    }

    public class PowerDensity{
        public int PowerDensityId { get; set; }
        public int LedId { get; set; }
        [ForeignKey("LedId")]
        public virtual Led Led { get; set; }
        public double Current { get; set; }
        public double PowerDenisty { get; set; }
    }

    public class Led {
        public int LedId { get; set; }
        public Wavelength Wavelength { get; set; }
        public virtual List<PowerDensity> PowerDensities { get; set; }

        public Led() {
            this.PowerDensities = new List<PowerDensity>();
        }
        
    }
}
