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

namespace ControllerProgrammer.ProgramForm.ViewModels {
    public class RecipeInputViewModel : ProgrammerViewModelBase {
        private IEventAggregator _eventAggregator;

        private double _led1DelayTime;
        private double _led2DelayTime;
        private double _led3DelayTime;

        private double _led1RunTime;
        private double _led2RunTime;
        private double _led3RunTime;

        private string _programStatus;

        private bool _controllerConnected;

        private IControllerDataManagment _controllerDataManagment;
        private IControllerManager _controllerManager;

        private ObservableCollection<PowerDensity> _lED1PowerDensites;
        private ObservableCollection<PowerDensity> _lED2PowerDensites;
        private ObservableCollection<PowerDensity> _lED3PowerDensites;
        public AsyncCommand ProgramDeviceCommand { get; private set; }
        public AsyncCommand LoadedCommand { get; private set; }

        public RecipeInputViewModel(IControllerDataManagment controllerDataManagment,IControllerManager controllerManager,IEventAggregator eventAggregator) {
            this._eventAggregator = eventAggregator;
            this._controllerDataManagment = controllerDataManagment;
            this._controllerManager = controllerManager;
            this.LoadedCommand = new AsyncCommand(this.LoadAsync);
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


        public async Task ProgramControllerHandler() {

        }

        public async Task LoadAsync() {
            await Task.Run(() => {
                this.ControllerConnected = false;
                this.ProgramStatus = "Not Connected";
                this.Led1DelayTime = 10;
                this.Led2DelayTime = 60;
                this.Led3DelayTime = 120;

                this.Led1RunTime = 120;
                this.Led2RunTime = 180;
                this.Led3RunTime = 30;

                var led1Pd = this._controllerDataManagment.GetPowerDensities(1);
                this.LED1PowerDensites = new ObservableCollection<PowerDensity>(led1Pd);

                var led2Pd = this._controllerDataManagment.GetPowerDensities(2);
                this.LED3PowerDensites = new ObservableCollection<PowerDensity>(led2Pd);

                var led3Pd = this._controllerDataManagment.GetPowerDensities(3);
                this.LED3PowerDensites = new ObservableCollection<PowerDensity>(led3Pd);

            });
        }
    }
}
