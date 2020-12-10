using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControllerProgrammer.Common.Interfaces;
using ControllerProgrammer.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace ControllerProgrammer.Common.Services {
    public class ControllerDataManagment : IControllerDataManagment {
        private ProgrammerContext _context;

        public ControllerDataManagment(ProgrammerContext context) {
            this._context = context;
        }

        public Led GetLED(int led) {
            return this._context.Leds.Include(e => e.PowerDensities).FirstOrDefault(e => e.LedId == led);
        }

        public IEnumerable<PowerDensityDto> GetPowerDensities(int led) {
            return this._context.PowerDensities.Include(e => e.Led).Where(e => e.LedId == led).Select(pd => new PowerDensityDto() {
                Current = ((double)pd.Current) / 1000,
                PowerDensity = ((double)pd.PowerDenisty) / 1000,
                Wavelength = pd.Led.Wavelength,
                LedId=pd.Led.LedId,
                PowerDensityId=pd.PowerDensityId
            });
        }

        public IEnumerable<PowerDensityDto> GetDensityTable() {
            return this._context.PowerDensities.Include(e => e.Led).Select(pd => new PowerDensityDto() {
                Current = ((double)pd.Current) / 1000,
                PowerDensity = ((double)pd.PowerDenisty) / 1000,
                Wavelength = pd.Led.Wavelength,
                LedId = pd.LedId,
                PowerDensityId = pd.PowerDensityId
            });
        }


        public DataManagmentResponse SavePowerDensities(int id, IEnumerable<PowerDensity> powerDensities) {
            var led = this._context.Leds.Include(e => e.PowerDensities).FirstOrDefault(e => e.LedId == id);
            if (led != null) {
                foreach(var pd in powerDensities) {
                    var check = led.PowerDensities.FirstOrDefault(e => e.Current == pd.Current);
                    if (check != null) {
                        check.PowerDenisty = pd.PowerDenisty;
                        this._context.Update(check);
                        this._context.SaveChanges();
                    } else {
                        pd.Led = led;
                        this._context.Add(pd);
                        this._context.SaveChanges();
                    }
                }
                return new DataManagmentResponse(true, "Success: Values updated");
            } else {
                return new DataManagmentResponse(false, "Internal Error: Could not find LED");
            }
        }

    }
}
