using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Regions;

namespace ControllerProgrammer.ProgramForm.ViewModels {
    public class ProgramFormViewModel : BindableBase, IRegionMemberLifetime {
        public bool KeepAlive => true;
    }
}
