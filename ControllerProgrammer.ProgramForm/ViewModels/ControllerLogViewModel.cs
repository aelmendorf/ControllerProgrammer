using System;
using System.Collections.Generic;
using System.Text;
using Prism.Mvvm;
using DevExpress.Mvvm;
using System.Linq;
using System.Collections.ObjectModel;
using ControllerProgrammer.Common;
using ControllerProgrammer.Data;
using ControllerProgrammer.Common.Interfaces;
using Prism.Events;
using System.Threading.Tasks;
using ControllerProgrammer.ProgramForm.Internal;

namespace ControllerProgrammer.ProgramForm.ViewModels {
    public class ControllerLogViewModel : ProgrammerViewModelBase {
        protected IDispatcherService _dispatcher { get => ServiceContainer.GetService<IDispatcherService>("LogDispatcherService"); }
        protected IMessageBoxService _messageService { get => ServiceContainer.GetService<IMessageBoxService>("LogMessageService"); }

        private IControllerManager _controller;
        private IEventAggregator _eventAggregator;

        private int _boardCycleCount;
        private int _led1CycleCount;
        private int _led2CycleCount;
        private int _led3CycleCount;
        private bool _controllerConnected;
        private string _controllerResponse;
        private bool _isLoaded;

        public DelegateCommand RequestLogCommand { get; private set; }
        public AsyncCommand LoadedCommand { get; private set; }

        public ControllerLogViewModel(IControllerManager controller,IEventAggregator eventAggregator) {
            this._controller = controller;
            this._eventAggregator = eventAggregator;

            this._eventAggregator.GetEvent<USBConnectedEvent>().Subscribe(this.USBConnectedHandler);
            this._eventAggregator.GetEvent<USBDisconnectedEvent>().Subscribe(this.USBDisconnectedHandler);
            this._eventAggregator.GetEvent<RecieveLogEvent>().Subscribe(this.RecieveResponseHandler);

            this.RequestLogCommand = new DelegateCommand(this.RequestLogHandler);
            this.LoadedCommand = new AsyncCommand(this.LoadAsync);
        }

        public override bool KeepAlive => true;

        public int BoardCycleCount {
            get => this._boardCycleCount;
            set => SetProperty(ref this._boardCycleCount, value);
        }

        public int Led1CycleCount {
            get => this._led1CycleCount;
            set => SetProperty(ref this._led1CycleCount, value);
        }

        public int Led2CycleCount {
            get => this._led2CycleCount;
            set => SetProperty(ref this._led2CycleCount, value);
        }

        public int Led3CycleCount {
            get => this._led3CycleCount;
            set => SetProperty(ref this._led3CycleCount, value);
        }

        public bool ControllerConnected { 
            get => this._controllerConnected; 
            set => SetProperty(ref this._controllerConnected,value); 
        }
        
        public string ControllerResponse { 
            get => this._controllerResponse; 
            set => SetProperty(ref this._controllerResponse,value);
        }

        private void RecieveResponseHandler(string response) {
            var logValues = response.Split(';');
            for (int i = 0; i < logValues.Count(); i++) {
                var values = logValues[i].Split(',');
                if (values.Count() == 4) {
                    this._dispatcher.BeginInvoke(() => {
                        this.BoardCycleCount = Convert.ToInt32(values[0]);
                        this.Led1CycleCount = Convert.ToInt32(values[1]);
                        this.Led2CycleCount = Convert.ToInt32(values[2]);
                        this.Led3CycleCount = Convert.ToInt32(values[3]);
                        this.ControllerResponse = "Log Recieved: "+response;
                    });
                }
            }

        }

        private void RequestLogHandler() {
            var response = this._controller.RequestLog();
        }

        private void USBConnectedHandler() {
            this.ControllerConnected = true;
        }

        private void USBDisconnectedHandler() {
            this.ControllerConnected = false;
        }

        private async Task LoadAsync() {
            await Task.Run(() => {
                if (this._isLoaded) {
                    this.BoardCycleCount = 0;
                    this.Led1CycleCount = 0;
                    this.Led2CycleCount = 0;
                    this.Led3CycleCount = 0;
                    this.ControllerConnected = false;
                    this.ControllerResponse = "";
                    this._isLoaded = true;
                }
            });

        }
    }
}
