using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControllerProgrammer.Common;
using ControllerProgrammer.Common.Interfaces;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace ControllerProgrammer.ProgramForm.ViewModels {
    public class ProgramFormViewModel : ProgrammerViewModelBase {
        private IControllerManager _controller;
        private IEventAggregator _eventAggregator;

        public override bool KeepAlive => false;
    }
}
