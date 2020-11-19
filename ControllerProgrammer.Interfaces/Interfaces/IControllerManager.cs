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

        public Byte[] GetBuffer() {
            StringBuilder buffer = new StringBuilder();
            buffer.AppendFormat("{0},{1},{2};",this.Led1Delay,this.Led1RunTime,this.Led1Current);
            buffer.AppendFormat("{0},{1},{2};", this.Led2Delay, this.Led2RunTime, this.Led2Current);
            buffer.AppendFormat("{0},{1},{2};", this.Led3Delay, this.Led3RunTime, this.Led3Current);
            return Encoding.ASCII.GetBytes(buffer.ToString());
        }
    }

    public class ValueReadyEventArg : EventArgs {
        public string Response { get; set; }
    }

    public class ControllerManagerResponse {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }

        public ControllerManagerResponse(bool success,string message) {
            this.Success = success;
            this.Message = message;
        }
    }

    public interface IControllerManager {
        event EventHandler<ValueReadyEventArg> ValueReady;
        ControllerManagerResponse Connect();
        bool IsConnected();
        void Disconnect();
        ControllerManagerResponse ProgramController(ControllerRecipe recipe);

    }
}
