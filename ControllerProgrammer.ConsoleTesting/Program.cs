using System;
using System.Linq;
using ControllerProgrammer.Data.Model;
using Microsoft.EntityFrameworkCore;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Microsoft.Win32;
namespace ControllerProgrammer.ConsoleTesting {
    class Program {
        static void Main(string[] args) {
            //CreateLed();
            //Create();

            //PrintDensities();
            //PrintLEDs();
            //var ports = getPortByVPid("16C0", "047A");
            //foreach(var port in ports) {
            //    Console.WriteLine(port);
            //}
            //Console.WriteLine("Should be done maybe");
            //TestingUSB();
            TestingStringParse();

        }

        public static void TestingStringParse() {
            string test = "52;2,3,4;5,6,7;8,9,10;";
            var p = test.Split(';');
            
            Console.WriteLine("Values Below: ");
            foreach(var value in p) {
                var parameters = value.Split(',');
                Console.WriteLine("Not Split: {0}",value);
                Console.WriteLine("Split Values: ");
                foreach(var par in parameters) {
                    Console.WriteLine(par);
                }

            }
        }

        public static void TestingUSB() {
            var portNames = SerialPort.GetPortNames();
            var ports = getPortByVPid("16C0", "047A");
            if(ports.Count>0 && ports.Count == 1) {
                var usb = portNames.FirstOrDefault(e => e == ports.ElementAt(0));
                if (!string.IsNullOrEmpty(usb)) {
                    Console.WriteLine("USB Found on {0}",usb);
                } else {
                    Console.WriteLine("USB Not Found");
                }

            } else {
                Console.WriteLine("Too Many Devices Found");
            }

            //Console.WriteLine("Available Ports");
            //foreach(var portName in portNames) {
            //    Console.WriteLine(portName);
            //}
            
            //Console.WriteLine("Ports");
            //foreach (var p in ports) {
            //    Console.WriteLine(p);
            //}


            

            //if (ports.Count == 1 & ports.Count>0) {
            //    var port = portNames.FirstOrDefault(e => e == ports.ElementAt(0));

            //    Console.WriteLine("Port Found: {0}",port);
            //} else {
            //    Console.WriteLine("More than 1 device found");
            //}
            
            //SerialPort serialPort = new SerialPort();
            //serialPort.BaudRate = 38400;
            //serialPort.DataBits = 8;
            //serialPort.Parity = Parity.None;
            //serialPort.StopBits = StopBits.One;
            //serialPort.Handshake = Handshake.None;

            //serialPort.Open();
            //SerialPort.
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        private static List<string> getPortByVPid(String VID, String PID) {
            String pattern = String.Format("^VID_{0}.PID_{1}", VID, PID);
            Regex _rx = new Regex(pattern, RegexOptions.IgnoreCase);
            List<string> comports = new List<string>();
            RegistryKey rk1 = Registry.LocalMachine;
            RegistryKey rk2 = rk1.OpenSubKey("SYSTEM\\CurrentControlSet\\Enum");
            foreach (String s3 in rk2.GetSubKeyNames()) {
                RegistryKey rk3 = rk2.OpenSubKey(s3);
                foreach (String s in rk3.GetSubKeyNames()) {
                    if (_rx.Match(s).Success) {
                        RegistryKey rk4 = rk3.OpenSubKey(s);
                        foreach (String s2 in rk4.GetSubKeyNames()) {
                            RegistryKey rk5 = rk4.OpenSubKey(s2);
                            RegistryKey rk6 = rk5.OpenSubKey("Device Parameters");
                            comports.Add((string)rk6.GetValue("PortName"));
                        }
                    }
                }
            }
            return comports;
        }

        public static void CreateLed() {
            using var context = new ProgrammerContext();
            //context.Database.EnsureCreated();
           
            Led led1 = new Led();
            led1.LedId = 1;
            led1.Wavelength = Wavelength.W285;

            Led led2 = new Led();
            led2.LedId = 2;
            led2.Wavelength = Wavelength.W300;

            Led led3 = new Led();
            led3.LedId = 3;
            led3.Wavelength = Wavelength.W310;

            context.Add(led1);
            context.Add(led2);
            context.Add(led3);
            context.SaveChanges();
            Console.WriteLine("Led Created");
        }

        public static void Create() {
            using var context = new ProgrammerContext();
            //context.Database.EnsureCreated();
            Random rand = new Random();
            foreach (var led in context.Leds) {
                for (int i = 0; i < 20; i++) {
                    PowerDensity pd = new PowerDensity();
                    pd.Led = led;
                    pd.Current = i * 1;
                    pd.PowerDenisty = i * rand.Next(1,10);
                    context.Add(pd);

                }
                context.SaveChanges();
                Console.WriteLine("Added");
                //Console.WriteLine("LED WL {0}",led.Wavelength);
            }
            Console.WriteLine("Should be done");

            //context.
        }

        public static void PrintLEDs() {
            using var context = new ProgrammerContext();
            //context.Database.EnsureCreated();
            foreach (var led in context.Leds) {
                Console.WriteLine("LED WL {0}",led.Wavelength);
            }
            Console.WriteLine("Should be done");
        }

        public static void PrintDensities() {
            using var context = new ProgrammerContext();
            var connString=context.Database.GetConnectionString();
            Console.WriteLine("Connection String: {0}", connString);
            var led=context.Leds.Include(e=>e.PowerDensities).FirstOrDefault(e => e.LedId == 1);
            Console.WriteLine("LED: {0}",led.Wavelength);
            foreach(var pd in led.PowerDensities.OrderBy(e=>e.Current)) {
                Console.WriteLine("LEDId: {0} Current: {1} PowerDensity: {2}", pd.LedId,pd.Current, pd.PowerDenisty);
            }
        }
    }
}
