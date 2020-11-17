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

    public interface IControllerDataManagment {
        IEnumerable<PowerDensity> GetPowerDensities(int led);
        Led GetLED(int LED);
        DataManagmentResponse SavePowerDensities(int led, IEnumerable<PowerDensity> powerDensities);
    }
}
