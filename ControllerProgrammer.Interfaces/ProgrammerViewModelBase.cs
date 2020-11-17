using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Regions;
using Prism.Mvvm;


namespace ControllerProgrammer.Common {
    public abstract class ProgrammerViewModelBase : BindableBase, IRegionMemberLifetime {
        public abstract bool KeepAlive { get; }
    }
}
