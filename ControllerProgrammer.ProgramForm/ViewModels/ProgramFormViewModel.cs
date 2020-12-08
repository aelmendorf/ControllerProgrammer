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
using ControllerProgrammer.ProgramForm.Internal;

namespace ControllerProgrammer.ProgramForm.ViewModels {
    public class ProgramFormViewModel : ProgrammerViewModelBase {
        protected IDispatcherService DispatcherService { get => ServiceContainer.GetService<IDispatcherService>("DispatcherService"); }
        protected IMessageBoxService MessageService { get => ServiceContainer.GetService<IMessageBoxService>("MessageService"); }

        private IControllerManager _controller;
        private IEventAggregator _eventAggregator;

        private string _connectButtonText;
        private string _connectionStatus;
        private bool _loaded;

        public DelegateCommand ConnectCommand { get; private set; }
        public AsyncCommand InitializeCommand { get; private set; }

        public ProgramFormViewModel(IControllerManager controllerManager,IEventAggregator eventAggregator) {
            this._controller = controllerManager;
            this._eventAggregator = eventAggregator;
            this._controller.ValueReady += this._controllerManager_ValueReady;
            this.ConnectCommand = new DelegateCommand(this.ConnectHandler);
            this.InitializeCommand = new AsyncCommand(this.LoadAsync);
        }

        public override bool KeepAlive => true;
        public string ConnectButtonText { 
            get => this._connectButtonText;
            set => SetProperty(ref this._connectButtonText, value);
        }

        public string ConnectionStatus { 
            get => this._connectionStatus; 
            set => SetProperty(ref this._connectionStatus,value);
        }

        private void ConnectHandler() {
            if (this._controller.IsConnected()) {
                this._controller.Disconnect();
                this.ConnectButtonText = "Connect";
                this._eventAggregator.GetEvent<USBDisconnectedEvent>().Publish();
            } else {
                var response=this._controller.Connect();
                if (response.Success) {
                    this.ConnectionStatus = "Connected";
                    this.ConnectButtonText = "Disconnect";
                    this._eventAggregator.GetEvent<USBConnectedEvent>().Publish();
                } else {
                    this.MessageService.ShowMessage(response.Message, "Error Connecting", MessageButton.OK, MessageIcon.Error);
                }
            }
        }

        private async Task LoadAsync() {
            if (!this._loaded) {
                this._loaded = true;
                this._connectButtonText = "Connect";
            }
        }

        private void _controllerManager_ValueReady(object sender, ValueReadyEventArg e) {
            string response = e.Response;
            if (response.Contains('s')) {
                this._eventAggregator.GetEvent<RecieveProgrammedEvent>().Publish(response);
            } else if (response.Contains('l')) {
                this._eventAggregator.GetEvent<RecieveLogEvent>().Publish(response);
            } else {
                this._eventAggregator.GetEvent<RecieveRecipeEvent>().Publish(response);

            }
        }
    }
}
