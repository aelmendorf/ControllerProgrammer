using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControllerProgrammer.Data.Model;
using ControllerProgrammer.Common;
using AsyncAwaitBestPractices.MVVM;
using Prism.Events;
using System.Collections.ObjectModel;
using ControllerProgrammer.Common.Interfaces;
using System.Windows.Threading;
using ControllerProgrammer.ProgramForm.Internal;
using Prism.Commands;

namespace ControllerProgrammer.ProgramForm.ViewModels {
    public class RecipeInputViewModel : ProgrammerViewModelBase {
        private IEventAggregator _eventAggregator;
        private Dispatcher _dispatcher;
        private double _led1DelayTime;
        private double _led2DelayTime;
        private double _led3DelayTime;

        private double _led1RunTime;
        private double _led2RunTime;
        private double _led3RunTime;

        private string _programStatus;
        private string _connectionStatus;
        private string _connectButtonText;

        private bool _controllerConnected;

        private IControllerDataManagment _controllerDataManagment;
        private IControllerManager _controllerManager;

        private ObservableCollection<PowerDensity> _lED1PowerDensites;
        private ObservableCollection<PowerDensity> _lED2PowerDensites;
        private ObservableCollection<PowerDensity> _lED3PowerDensites;
        private PowerDensity _led1SelectedDensity;
        private PowerDensity _led2SelectedDensity;
        private PowerDensity _led3SelectedDensity;
        public DelegateCommand ProgramDeviceCommand { get; private set; }
        public AsyncCommand LoadedCommand { get; private set; }
        public DelegateCommand ConnectCommand { get; private set; }

        public RecipeInputViewModel(IControllerDataManagment controllerDataManagment,IControllerManager controllerManager,IEventAggregator eventAggregator) {
            this._eventAggregator = eventAggregator;
            this._controllerDataManagment = controllerDataManagment;
            this._controllerManager = controllerManager;
            this._dispatcher = Dispatcher.CurrentDispatcher;
            //this._eventAggregator.GetEvent<USBConnectedEvent>().Subscribe(this.UpdateConnected);
            this._controllerManager.ValueReady += this._controllerManager_ValueReady;
            this.LoadedCommand = new AsyncCommand(this.LoadAsync);
            this.ConnectCommand = new DelegateCommand(this.ConnectHandler);
            this.ProgramDeviceCommand = new DelegateCommand(this.ProgramControllerHandler);
        }

        private void _controllerManager_ValueReady(object sender, ValueReadyEventArg e) {
            string response = e.Response;
            if (response.Contains('s')) {
                this._dispatcher.Invoke(() => {
                    this.ProgramStatus = "Success";
                });
            } else {
                var ledParameters = response.Split(';');
                StringBuilder errors = new StringBuilder();
                for (int i = 0; i < ledParameters.Count(); i++) {
                    var parameters = ledParameters[i].Split(',');
                    if (parameters.Count() == 3) {
                        switch (i) {
                            case 0: {
                                    this._dispatcher.Invoke(() => {
                                        this.Led1DelayTime = Convert.ToInt64(parameters[0]);
                                        this.Led1RunTime = Convert.ToInt64(parameters[1]);
                                        var current = Convert.ToInt64(parameters[2]);
                                        this.Led1SelectedDensity = this.LED1PowerDensites.FirstOrDefault(e => e.Current == current);
                                    });
                                    break;
                                }
                            case 1: {
                                    this._dispatcher.Invoke(() => {
                                        this.Led2DelayTime = Convert.ToInt64(parameters[0]);
                                        this.Led2RunTime = Convert.ToInt64(parameters[1]);
                                        var current = Convert.ToInt64(parameters[2]);
                                        this.Led2SelectedDensity = this.LED2PowerDensites.FirstOrDefault(e => e.Current == current);
                                    });
                                    break;
                                }
                            case 2: {
                                    this._dispatcher.Invoke(() => {
                                        this.Led3DelayTime = Convert.ToInt64(parameters[0]);
                                        this.Led3RunTime = Convert.ToInt64(parameters[1]);
                                        var current = Convert.ToInt64(parameters[2]);
                                        this.Led3SelectedDensity = this.LED3PowerDensites.FirstOrDefault(e => e.Current == current);
                                    });
                                    break;
                                }
                            default: {
                                    errors.AppendLine("Error Index Out Of Range");
                                    break;
                                }
                        }

                    } else {
                        errors.AppendLine("Error: Size > 3");
                    }
                }
            }

        }

        public override bool KeepAlive => false;

        public double Led1DelayTime { 
            get => this._led1DelayTime;
            set => SetProperty(ref this._led1DelayTime, value);
        }

        public double Led2DelayTime {
            get => this._led2DelayTime;
            set => SetProperty(ref this._led2DelayTime, value);
        }

        public double Led3DelayTime { 
            get => this._led3DelayTime;
            set => SetProperty(ref this._led3DelayTime, value);
        }

        public double Led1RunTime { 
            get => this._led1RunTime;
            set => SetProperty(ref this._led1RunTime, value);
        }

        public double Led2RunTime { 
            get => this._led2RunTime;
            set => SetProperty(ref this._led2RunTime, value);
        }

        public double Led3RunTime {
            get => this._led3RunTime; 
            set => SetProperty(ref this._led3RunTime, value);
        }

        public ObservableCollection<PowerDensity> LED2PowerDensites { 
            get => this._lED2PowerDensites;
            set => SetProperty(ref this._lED2PowerDensites, value);
        }

        public ObservableCollection<PowerDensity> LED1PowerDensites {
            get => this._lED1PowerDensites;
            set => SetProperty(ref this._lED1PowerDensites, value);
        }

        public ObservableCollection<PowerDensity> LED3PowerDensites {
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

        public PowerDensity Led1SelectedDensity { 
            get => this._led1SelectedDensity;
            set => SetProperty(ref this._led1SelectedDensity, value);
        }
        
        public PowerDensity Led2SelectedDensity { 
            get => this._led2SelectedDensity;
            set => SetProperty(ref this._led2SelectedDensity, value);
        }
        
        public PowerDensity Led3SelectedDensity {
            get => this._led3SelectedDensity;
            set => SetProperty(ref this._led3SelectedDensity, value);
        }

        public void UpdateConnected() {
            this.ControllerConnected = true;
        }

        public void ConnectHandler() {
            if (!this._controllerManager.IsConnected()) {
                var response = this._controllerManager.Connect();
                if (response.Success) {
                    this.ControllerConnected = true;
                    this.ConnectButtonText = "Disconnect";
                    this.ConnectionStatus = "Connected to Controller";
                    this._controllerManager.RequestData();
                } else {
                    this.ControllerConnected = false;
                    this.ConnectButtonText = "Connect";
                    this.ProgramStatus = "Not Connected";
                    this.ConnectionStatus = response.Message;
                }
            } else {
                this._controllerManager.Disconnect();
                this.ControllerConnected = false;
                this.ConnectButtonText = "Connect";
                this.ProgramStatus = "Not Connected";
            }
        }

        public void ProgramControllerHandler() {
            ControllerRecipe recipe = new ControllerRecipe();
            recipe.Led1Delay = this.Led1DelayTime;
            recipe.Led1RunTime = this.Led1RunTime;
            recipe.Led1Current = this.Led1SelectedDensity.Current;

            recipe.Led2Delay = this.Led2DelayTime;
            recipe.Led2RunTime = this.Led2RunTime;
            recipe.Led2Current = this.Led2SelectedDensity.Current;

            recipe.Led3Delay = this.Led3DelayTime;
            recipe.Led3RunTime = this.Led3RunTime;
            recipe.Led3Current = this.Led3SelectedDensity.Current;

            this._controllerManager.ProgramController(recipe);
        }

        public async Task LoadAsync() {
            await Task.Run(() => {
                this.ControllerConnected = false;
                this.ProgramStatus = "Not Connected";
                this.ConnectionStatus = "Not Connected";
                this.ConnectButtonText = "Connect";
                this.Led1DelayTime = 10;
                this.Led2DelayTime = 60;
                this.Led3DelayTime = 120;

                this.Led1RunTime = 120;
                this.Led2RunTime = 180;
                this.Led3RunTime = 30;

                var led1Pd = this._controllerDataManagment.GetPowerDensities(1);
                this.LED1PowerDensites = new ObservableCollection<PowerDensity>(led1Pd);
                this.LED1PowerDensites.OrderBy(e => e.Current);
                this.Led1SelectedDensity = this.LED1PowerDensites.First();

                var led2Pd = this._controllerDataManagment.GetPowerDensities(2);
                this.LED2PowerDensites = new ObservableCollection<PowerDensity>(led2Pd);
                this.LED2PowerDensites.OrderBy(e => e.Current);
                this.Led2SelectedDensity = this.LED2PowerDensites.First();

                var led3Pd = this._controllerDataManagment.GetPowerDensities(3);
                this.LED3PowerDensites = new ObservableCollection<PowerDensity>(led3Pd);
                this.LED3PowerDensites.OrderBy(e => e.Current);
                this.Led3SelectedDensity = this.LED3PowerDensites.First();

            });
        }
    }
}
