using System;
using System.Collections.Generic;
using System.Text;
using Prism.Mvvm;
using DevExpress.Mvvm;
using ControllerProgrammer.Common;
using ControllerProgrammer.Data;
using ControllerProgrammer.Common.Interfaces;
using Prism.Events;

namespace ControllerProgrammer.ProgramForm.ViewModels {
    public class ControllerLogViewModel : ProgrammerViewModelBase {
        private IControllerManager _controller;
        private IEventAggregator _eventAggregator;
        public override bool KeepAlive => throw new NotImplementedException();
    }
}
