using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControllerProgrammer.Data.Model;
using ControllerProgrammer.Common;
using Prism.Events;
using System.Collections.ObjectModel;
using ControllerProgrammer.Common.Interfaces;
using System.Windows.Threading;
using ControllerProgrammer.ProgramForm.Internal;
using DevExpress.Mvvm;

namespace ControllerProgrammer.ProgramForm.ViewModels {
    public class RecipeInputViewModel : ProgrammerViewModelBase {
        
        protected IDispatcherService _dispatcher { get => ServiceContainer.GetService<IDispatcherService>("RecipeDispatcherService"); }
        protected IMessageBoxService _messageService { get => ServiceContainer.GetService<IMessageBoxService>("RecipeMessageService"); }

        private IControllerDataManagment _controllerDataManager;
        private IControllerManager _controller;
        private IEventAggregator _eventAggregator;

        private int _boardCycleTime;
        private int _led1DelayTime;
        private int _led2DelayTime;
        private int _led3DelayTime;
        private int _led1RunTime;
        private int _led2RunTime;
        private int _led3RunTime;
        private double _led1Dosage;
        private double _led2Dosage;
        private double _led3Dosage;
        private string _programStatus;
        private string _connectionStatus;
        private string _connectButtonText;
        private bool _controllerConnected;

        private ObservableCollection<PowerDensityDto> _lED1PowerDensites;
        private ObservableCollection<PowerDensityDto> _lED2PowerDensites;
        private ObservableCollection<PowerDensityDto> _lED3PowerDensites;
        private PowerDensityDto _led1SelectedDensity;
        private PowerDensityDto _led2SelectedDensity;
        private PowerDensityDto _led3SelectedDensity;
        private bool _isSplashScreenShown;
        public DelegateCommand ProgramDeviceCommand { get; private set; }
        public DelegateCommand ReadDeviceMemoryCommand { get; private set; }
        public AsyncCommand LoadedCommand { get; private set; }
        public DelegateCommand ConnectCommand { get; private set; }

        public RecipeInputViewModel(IControllerDataManagment controllerDataManagment,IControllerManager controllerManager,IEventAggregator eventAggregator) {
            this._eventAggregator = eventAggregator;
            this._controllerDataManager = controllerDataManagment;
            this._controller = controllerManager;
            this._eventAggregator.GetEvent<USBConnectedEvent>().Subscribe(this.UsbConnectedHandler);
            this._eventAggregator.GetEvent<USBDisconnectedEvent>().Subscribe(this.UsbDisconnectedHandler);
            //this._eventAggregator.GetEvent<USBdi>().Subscribe(this.UsbConnectedHandler);
            // this._controller.ValueReady += this._controllerManager_ValueReady;
            this._eventAggregator.GetEvent<RecieveRecipeEvent>().Subscribe(this.RecieveRecipeHandler);
            this._eventAggregator.GetEvent<RecieveProgrammedEvent>().Subscribe(this.RecieveProgrammedHandler);
            this.LoadedCommand = new AsyncCommand(this.LoadAsync);
            this.ConnectCommand = new DelegateCommand(this.ConnectHandler);
            this.ProgramDeviceCommand = new DelegateCommand(this.ProgramControllerHandler);
            this.ReadDeviceMemoryCommand = new DelegateCommand(this.ReadDeviceMemoryHandler);
        }

        

        public override bool KeepAlive => false;

        public int Led1DelayTime { 
            get => this._led1DelayTime;
            set => SetProperty(ref this._led1DelayTime, value);
        }

        public int Led2DelayTime {
            get => this._led2DelayTime;
            set => SetProperty(ref this._led2DelayTime, value);
        }

        public int Led3DelayTime { 
            get => this._led3DelayTime;
            set => SetProperty(ref this._led3DelayTime, value);
        }

        public int Led1RunTime { 
            get => this._led1RunTime;
            set{
                SetProperty(ref this._led1RunTime, value);
                if (this.Led1SelectedDensity != null) {
                    this.Led1Dosage = (this._led1RunTime * 60) + this.Led1SelectedDensity.PowerDensity;
                }
                
            }
        }

        public int Led2RunTime { 
            get => this._led2RunTime;
            set {
                SetProperty(ref this._led2RunTime, value);
                if (this.Led2SelectedDensity != null) {
                    this.Led2Dosage = (this._led2RunTime * 60) + this.Led2SelectedDensity.PowerDensity;
                }
            }
        }

        public int Led3RunTime {
            get => this._led3RunTime;
            set {
                SetProperty(ref this._led3RunTime, value);
                if (this.Led3SelectedDensity != null) {
                    this.Led3Dosage = (this._led3RunTime * 60) + this.Led3SelectedDensity.PowerDensity;
                }
            }
        }

        public ObservableCollection<PowerDensityDto> LED2PowerDensites { 
            get => this._lED2PowerDensites;
            set => SetProperty(ref this._lED2PowerDensites, value);
        }

        public ObservableCollection<PowerDensityDto> LED1PowerDensites {
            get => this._lED1PowerDensites;
            set => SetProperty(ref this._lED1PowerDensites, value);
        }

        public ObservableCollection<PowerDensityDto> LED3PowerDensites {
            get => this._lED3PowerDensites;
            set => SetProperty(ref this._lED3PowerDensites, value);
        }

        public string ProgramStatus { 
            get => this._programStatus;
            set => SetProperty(ref this._programStatus, value); 
        }

        public bool ControllerConnected {
            get => this._controllerConnected;
            set => SetProperty(ref this._controllerConnected, value);
        }

        public string ConnectionStatus { 
            get => this._connectionStatus; 
            set => SetProperty(ref this._connectionStatus,value);
        }

        public string ConnectButtonText { 
            get => this._connectButtonText;
            set => SetProperty(ref this._connectButtonText,value);
        }

        public PowerDensityDto Led1SelectedDensity { 
            get => this._led1SelectedDensity;
            set {
                SetProperty(ref this._led1SelectedDensity, value);
                this.Led1Dosage = (this.Led1RunTime * 60) * this.Led1SelectedDensity.PowerDensity;
            } 
        }
        
        public PowerDensityDto Led2SelectedDensity { 
            get => this._led2SelectedDensity;
            set {
                SetProperty(ref this._led2SelectedDensity, value);
                this.Led2Dosage = (this.Led2RunTime * 60) * this.Led2SelectedDensity.PowerDensity;
            }
        }
        
        public PowerDensityDto Led3SelectedDensity {
            get => this._led3SelectedDensity;
            set {
                SetProperty(ref this._led3SelectedDensity, value);
                this.Led3Dosage = (this.Led3RunTime*60) * this.Led3SelectedDensity.PowerDensity;
            }
        }

        public int BoardCycleTime { 
            get => this._boardCycleTime;
            set => SetProperty(ref this._boardCycleTime, value);
        }
        
        public bool IsSplashScreenShown { 
            get => this._isSplashScreenShown;
            set => SetProperty(ref this._isSplashScreenShown, value);
        }
        public double Led1Dosage { 
            get => this._led1Dosage; 
            set => SetProperty(ref this._led1Dosage,value);
        }

        public double Led2Dosage { 
            get => this._led2Dosage;
            set => SetProperty(ref this._led2Dosage, value);
        }

        public double Led3Dosage { 
            get => this._led3Dosage;
            set => SetProperty(ref this._led3Dosage, value);
        }

        public void UpdateConnected() {
            this.ControllerConnected = true;
        }

        public void ConnectHandler() {
            if (!this._controller.IsConnected()) {
                var response = this._controller.Connect();
                if (response.Success) {
                    this.ControllerConnected = true;
                    this.ConnectButtonText = "Disconnect";
                    this.ConnectionStatus = "Connected to Controller";
                    this._controller.RequestRecipe();
                } else {
                    this.ControllerConnected = false;
                    this.ConnectButtonText = "Connect";
                    this.ProgramStatus = "Not Connected";
                    this.ConnectionStatus = response.Message;
                }
            } else {
                this._controller.Disconnect();
                this.ControllerConnected = false;
                this.ConnectButtonText = "Connect";
                this.ProgramStatus = "Not Connected";
            }
        }

        public void ProgramControllerHandler() {
            ControllerRecipe recipe = new ControllerRecipe();
            recipe.CycleTime = this.BoardCycleTime;
            recipe.Led1Delay = this.Led1DelayTime;
            recipe.Led1RunTime = this.Led1RunTime;
            recipe.Led1Current = (int)(this.Led1SelectedDensity.Current*1000);

            recipe.Led2Delay = this.Led2DelayTime;
            recipe.Led2RunTime = this.Led2RunTime;
            recipe.Led2Current = (int)(this.Led2SelectedDensity.Current * 1000);

            recipe.Led3Delay = this.Led3DelayTime;
            recipe.Led3RunTime = this.Led3RunTime;
            recipe.Led3Current = (int)(this.Led3SelectedDensity.Current * 1000);

            var response=this._controller.ProgramController(recipe);

        }

        private void UsbConnectedHandler() {
            this.ControllerConnected = true;
            this.ProgramStatus = "Connected";
        }

        private void UsbDisconnectedHandler() {
            this.ControllerConnected = false;
            this.ProgramStatus = "Disconnected";
        }

        private void ReadDeviceMemoryHandler() {
            var response=this._controller.RequestRecipe();
            if (response.Success) {
                this.ProgramStatus = "Success";
            } else {
                this.ProgramStatus = response.Message;
            }
        }

        private void RecieveProgrammedHandler(string response) {
            this.ProgramStatus = "Device Programmed Succesfully: "
                +Environment.NewLine
                +"Data:"
                +response;
        }

        private void RecieveRecipeHandler(string response) {
            var ledParameters = response.Split(';');
            StringBuilder errors = new StringBuilder();
            for (int i = 0; i < ledParameters.Count(); i++) {
                var parameters = ledParameters[i].Split(',');
                if (parameters.Count() == 3) {
                    switch (i) {
                        case 1: {
                                this._dispatcher.BeginInvoke(() => {
                                    this.Led1DelayTime = Convert.ToInt32(parameters[0]);
                                    this.Led1RunTime = Convert.ToInt32(parameters[1]);
                                    var current = (((double)Convert.ToInt32(parameters[2]))/1000);
                                    this.Led1SelectedDensity = this.LED1PowerDensites.FirstOrDefault(e => e.Current == current);
                                });
                                break;
                            }
                        case 2: {
                                this._dispatcher.BeginInvoke(() => {
                                    this.Led2DelayTime = Convert.ToInt32(parameters[0]);
                                    this.Led2RunTime = Convert.ToInt32(parameters[1]);
                                    var current = (((double)Convert.ToInt32(parameters[2])) / 1000);
                                    this.Led2SelectedDensity = this.LED2PowerDensites.FirstOrDefault(e => e.Current == current);
                                });
                                break;
                            }
                        case 3: {
                                this._dispatcher.BeginInvoke(() => {
                                    this.Led3DelayTime = Convert.ToInt32(parameters[0]);
                                    this.Led3RunTime = Convert.ToInt32(parameters[1]);
                                    //var current = Convert.ToInt32(parameters[2]);
                                    var current = (((double)Convert.ToInt32(parameters[2])) / 1000);
                                    this.Led3SelectedDensity = this.LED3PowerDensites.FirstOrDefault(e => e.Current == current);
                                });
                                break;
                            }
                        default: {
                                errors.AppendLine("Error Index Out Of Range");
                                break;
                            }
                    }
                } else if (parameters.Count() == 1) {
                    this._dispatcher.BeginInvoke(() => {
                        try {
                            this.BoardCycleTime = Convert.ToInt32(parameters[0]);
                        } catch { }

                    });
                } else {
                    errors.AppendLine("Error: Size > 3");
                }
            }
        }

        public async Task LoadAsync() {
            await Task.Run(() => {
                this._dispatcher.BeginInvoke(() => { this.IsSplashScreenShown = true; });
                this.ControllerConnected = false;
                this.ProgramStatus = "Not Connected";
                this.ConnectionStatus = "Not Connected";
                this.ConnectButtonText = "Connect";
                this.BoardCycleTime = 0;
                this.Led1DelayTime = 0;
                this.Led2DelayTime = 0;
                this.Led3DelayTime = 0;

                this.Led1RunTime = 0;
                this.Led2RunTime = 0;
                this.Led3RunTime = 0;

                var led1Pd = this._controllerDataManager.GetPowerDensities(1);
                this.LED1PowerDensites = new ObservableCollection<PowerDensityDto>(led1Pd);
                this.LED1PowerDensites.OrderBy(e => e.Current);
                this.Led1SelectedDensity = this.LED1PowerDensites.First();

                var led2Pd = this._controllerDataManager.GetPowerDensities(2);
                this.LED2PowerDensites = new ObservableCollection<PowerDensityDto>(led2Pd);
                this.LED2PowerDensites.OrderBy(e => e.Current);
                this.Led2SelectedDensity = this.LED2PowerDensites.First();

                var led3Pd = this._controllerDataManager.GetPowerDensities(3);
                this.LED3PowerDensites = new ObservableCollection<PowerDensityDto>(led3Pd);
                this.LED3PowerDensites.OrderBy(e => e.Current);
                this.Led3SelectedDensity = this.LED3PowerDensites.First();
                this._dispatcher.BeginInvoke(() => { this.IsSplashScreenShown = false; });

            });
        }
    }
}
