using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ControllerProgrammer.Common.Interfaces;
using System.IO.Ports;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace ControllerProgrammer.Common.Services {
    public class ControllerManager : IControllerManager {
        public event EventHandler<ValueReadyEventArg> ValueReady;
        public static string VID= "16C0";
        public static string PID = "047A";
        public static Parity Parity = Parity.None;
        public static StopBits StopBits = StopBits.One;
        public static int BaudRate = 38400;
        public static int DataBits = 8;

        private string _response;
        private SerialPort _controller;

        public ControllerManager() {
            this._controller = new SerialPort();
            this._controller.BaudRate = ControllerManager.BaudRate;
            this._controller.Parity = ControllerManager.Parity;
            this._controller.StopBits = ControllerManager.StopBits;
            this._controller.DataBits = ControllerManager.DataBits;
            this._controller.DataReceived += this._controller_DataReceived;
            //this._controller.WriteTimeout = 1000;
            //this._controller.ReadTimeout = 1000;
        }

        private void _controller_DataReceived(object sender, SerialDataReceivedEventArgs e) {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadTo("\r");
            EventHandler<ValueReadyEventArg> handler = ValueReady;
            if (handler != null) {
                handler.Invoke(this, new ValueReadyEventArg() { Response = indata });
            }
        }

        public ControllerManagerResponse Connect() {
            var success = this.FindSerialPort();
            if (success) {
                try {
                    this._controller.Open();
                    return new ControllerManagerResponse(true, "Connected to "+this._controller.PortName);
                } catch (Exception e) {
                    return new ControllerManagerResponse(false, "Error: Connection Failed,Please see error below:") { Exception = e };
                }
            } else {
                return new ControllerManagerResponse(false, "Error: Could not find device");
            }
        }

        public void Disconnect() {
            this._controller.Close();
        }

        public bool FindSerialPort() {
            var regPorts = FindUSBCom();
            if(regPorts.Count>0 && regPorts.Count == 1) {
                var portNames = SerialPort.GetPortNames();
                var usb = portNames.FirstOrDefault(e => e == regPorts.ElementAt(0));
                if (!string.IsNullOrEmpty(usb)) {
                    this._controller.PortName = usb;
                    return true;
                } else {
                    return false;
                }
            } else {
                return false;
            }
        }

        public ControllerManagerResponse ProgramController(ControllerRecipe recipe) {
            if (this._controller.IsOpen) {
                //StringBuilder buffer = new StringBuilder();
                //buffer.AppendFormat("p;{0};{1},{2},{3};{4},{5},{6};{7},{8},{9};\r",
                //    recipe.Led1Delay,recipe.Led1RunTime,recipe.Led1Current,
                //    recipe.Led2Delay, recipe.Led2RunTime, recipe.Led2Current,
                //    recipe.Led3Delay, recipe.Led3RunTime, recipe.Led3Current);
                //var bytes=Encoding.ASCII.GetBytes(buffer.ToString());
                var buffer = recipe.GetBuffer();
                try {
                    this._controller.Write(buffer);
                    return new ControllerManagerResponse(true, "Success: Command Sent");
                } catch {
                    return new ControllerManagerResponse(false, "Error: Could not send command, please try reconnecting board");
                }

            }
            return new ControllerManagerResponse(false, "Error: Controller not connected");
        }

        public ControllerManagerResponse RequestData() {
            try {
                this._controller.Write("r");
                return new ControllerManagerResponse(true,"Success: Message sent");
            } catch {
                return new ControllerManagerResponse(false, "Error: Could not send command, please try reconnecting board");
            }

        }

        private List<string> FindUSBCom() {
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

        public bool IsConnected() {
            return this._controller.IsOpen;
        }
    }
}
