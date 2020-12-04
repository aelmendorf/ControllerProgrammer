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
using DevExpress.Mvvm;

namespace ControllerProgrammer.ProgramForm.ViewModels {
    public class ProgramFormViewModel : ProgrammerViewModelBase {
        protected IDispatcherService DispatcherService { get => ServiceContainer.GetService<IDispatcherService>("DispatcherService"); }
        protected IMessageBoxService MessageService { get => ServiceContainer.GetService<IMessageBoxService>("MessageService"); }

        private IControllerManager _controller;
        private IEventAggregator _eventAggregator;

        private string _connectButtonText;
        private string _connectionStatus;
        private bool _loaded;

        public AsyncCommand ConnectCommand { get; private set; }
        public AsyncCommand InitializeCommand { get; private set; }

        public ProgramFormViewModel(IControllerManager controllerManager,IEventAggregator eventAggregator) {
            this._controller = controllerManager;
            this._eventAggregator = eventAggregator;
            this.ConnectCommand = new AsyncCommand(this.ConnectHandler);
            this.InitializeCommand = new AsyncCommand(this.LoadAsync);
        }

        public override bool KeepAlive => false;
        public string ConnectButtonText { get => this._connectButtonText; set => this._connectButtonText = value; }
        public string ConnectionStatus { get => this._connectionStatus; set => this._connectionStatus = value; }

        private async Task ConnectHandler() {
            await Task.Run(() => {
                if (this._controller.IsConnected()) {
                    this._controller.Disconnect();
                    this.DispatcherService.BeginInvoke(() => this.ConnectButtonText = "Connect");
                } else {
                    var response=this._controller.Connect();
                    if (response.Success) {
                        this.ConnectionStatus = "Connected";
                        this.ConnectButtonText = "Disconnect";
                    } else {
                        this.MessageService.ShowMessage(response.Message, "Error Connecting", MessageButton.OK, MessageIcon.Error);
                    }
                }
            });
        }

        private async Task LoadAsync() {
            if (!this._loaded) {
                this._loaded = true;
                this._connectButtonText = "Connect";
            }
        }
    }
}
