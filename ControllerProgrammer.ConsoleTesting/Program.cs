using System;
using System.Linq;
using ControllerProgrammer.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace ControllerProgrammer.ConsoleTesting {
    class Program {
        static void Main(string[] args) {
            PrintDensities();
            //PrintLEDs();
        }

        public static void Create() {
            using var context = new ProgrammerContext();
            context.Database.EnsureCreated();
            foreach (var led in context.Leds) {
                for (int i = 0; i < 20; i++) {
                    PowerDensity pd = new PowerDensity();
                    pd.Led = led;
                    pd.Current = i * 1;
                    pd.PowerDenisty = i * .002;
                    context.Add(pd);

                }
                context.SaveChanges();
                Console.WriteLine("Added");
                //Console.WriteLine("LED WL {0}",led.Wavelength);
            }
            Console.WriteLine("Should be done");
            //Led led1 = new Led();
            //led1.LedId = 1;
            //led1.Wavelength = Wavelength.W285;

            //Led led2 = new Led();
            //led2.LedId = 2;
            //led2.Wavelength = Wavelength.W300;

            //Led led3 = new Led();
            //led3.LedId= 3;
            //led3.Wavelength = Wavelength.W310;

            //context.Add(led1);
            //context.Add(led2);
            //context.Add(led3);
            //context.SaveChanges();
            //Console.WriteLine("Should be done");
            //context.
        }

        public static void PrintLEDs() {
            using var context = new ProgrammerContext();
            context.Database.EnsureCreated();
            foreach (var led in context.Leds) {
                Console.WriteLine("LED WL {0}",led.Wavelength);
            }
            Console.WriteLine("Should be done");
        }

        public static void PrintDensities() {
            using var context = new ProgrammerContext();
            var led=context.Leds.Include(e=>e.PowerDensities).FirstOrDefault(e => e.LedId == 1);
            Console.WriteLine("LED: {0}",led.Wavelength);
            foreach(var pd in led.PowerDensities.OrderBy(e=>e.Current)) {
                Console.WriteLine("LEDId: {0} Current: {1} PowerDensity: {2}", pd.LedId,pd.Current, pd.PowerDenisty);
            }
        }
    }
}
