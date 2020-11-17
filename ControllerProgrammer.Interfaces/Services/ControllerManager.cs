using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ControllerProgrammer.Common.Interfaces;
using System.IO.Ports;

namespace ControllerProgrammer.Common.Services {
    public class ControllerManager : IControllerManager {
        private SerialPort _controller;

        public ControllerManager() {
            this._controller = new SerialPort();
        }

        public bool Connect() {
            throw new NotImplementedException();
        }

        public bool Disconnect() {
            throw new NotImplementedException();
        }

        public bool FindSerialPower() {
            throw new NotImplementedException();
        }

        public bool ProgramController(ControllerRecipe recipe) {
            throw new NotImplementedException();
        }
    }
}
