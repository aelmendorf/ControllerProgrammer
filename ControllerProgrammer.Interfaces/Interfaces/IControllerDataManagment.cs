using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControllerProgrammer.Data.Model;

namespace ControllerProgrammer.Common.Interfaces {
    public class DataManagmentResponse {
        bool Success { get; set; }
        string Message { get; set; }

        public DataManagmentResponse(bool success, string message) {
            this.Success = success;
            this.Message = message;
        }
    }

    public class PowerDensityDto {
        public int PowerDensityId { get; set; }
        public int LedId { get; set; }
        public Wavelength Wavelength { get; set; }
        public double Current { get; set; }
        public double PowerDensity { get; set; }

    }

    public interface IControllerDataManagment {
        IEnumerable<PowerDensityDto> GetPowerDensities(int led);
        IEnumerable<PowerDensityDto> GetDensityTable();
        Led GetLED(int LED);
        DataManagmentResponse SavePowerDensities(int led, IEnumerable<PowerDensity> powerDensities);
    }
}
