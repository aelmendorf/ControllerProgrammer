using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerProgrammer.Common.Interfaces {
    public class ControllerRecipe {
        public int CycleTime { get; set; }
        public int Led1Delay { get; set; }
        public int Led1RunTime { get; set; }
        public int Led1Current { get; set; }

        public int Led2Delay { get; set; }
        public int Led2RunTime { get; set; }
        public int Led2Current { get; set; }

        public int Led3Delay { get; set; }
        public int Led3RunTime { get; set; }
        public int Led3Current { get; set; }

        public string GetBuffer() {
            StringBuilder buffer = new StringBuilder();
            buffer.AppendFormat("p;{0};{1},{2},{3};{4},{5},{6};{7},{8},{9};\r",
                this.CycleTime,
                this.Led1Delay,this.Led1RunTime,this.Led1Current,
                this.Led2Delay, this.Led2RunTime, this.Led2Current,
                this.Led3Delay, this.Led3RunTime, this.Led3Current);
            //buffer.AppendFormat("{0};", this.CycleTime);
            //buffer.AppendFormat("{0},{1},{2};",this.Led1Delay,this.Led1RunTime,this.Led1Current);
            //buffer.AppendFormat("{0},{1},{2};", this.Led2Delay, this.Led2RunTime, this.Led2Current);
            //buffer.AppendFormat("{0},{1},{2};\r", this.Led3Delay, this.Led3RunTime, this.Led3Current);
            //return Encoding.ASCII.GetBytes(buffer.ToString());
            return buffer.ToString();
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
        ControllerManagerResponse RequestRecipe();
        ControllerManagerResponse RequestLog();

    }
}
