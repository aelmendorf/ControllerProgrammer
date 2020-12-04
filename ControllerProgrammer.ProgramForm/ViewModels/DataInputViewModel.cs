using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControllerProgrammer.Common;
using ControllerProgrammer.Data.Model;
using Prism.Events;
using DevExpress.Mvvm;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using ControllerProgrammer.ProgramForm.Internal;

namespace ControllerProgrammer.ProgramForm.ViewModels {
    public class DataInputViewModel : ProgrammerViewModelBase {
        protected IDispatcherService DispatcherService { get => ServiceContainer.GetService<IDispatcherService>("DensityTableDispatcher"); } 
        protected IMessageBoxService MessageService { get => ServiceContainer.GetService<IMessageBoxService>("DensityTableMessageService"); }

        private ProgrammerContext _context;
        private IEventAggregator _eventAggregator;

        private ObservableCollection<PowerDensity> _powerDensities;
        private bool _loaded;
        private bool _showPowerDensityLoading;
        private PowerDensity _selectedPowerDensity;

        //public AsyncCommand SaveCommand { get; private set; }
        //public AsyncCommand CancelCommand { get; private set; }
        public AsyncCommand LoadedCommand { get; private set; }
        public AsyncCommand CommitCommand { get; private set; }
        public AsyncCommand CancelEditCommand { get; private set; }

        public DataInputViewModel(ProgrammerContext context,IEventAggregator eventAggregator) {
            this._context = context;
            this._eventAggregator = eventAggregator;

            //this.SaveCommand = new AsyncCommand(this.SaveHandler);
            //this.CancelCommand = new AsyncCommand(this.CancelHandler);
            this.LoadedCommand = new AsyncCommand(this.LoadAsync);
            this.CommitCommand = new AsyncCommand(this.CommitHandler);
            this.CancelEditCommand = new AsyncCommand(this.CancelEditHandler);
        }

        public override bool KeepAlive => false;

        public ObservableCollection<PowerDensity> PowerDensities { 
            get => this._powerDensities; 
            set => SetProperty(ref this._powerDensities,value); 
        }

        public bool Loaded { 
            get => this._loaded;
            set => SetProperty(ref this._loaded, value);
        }

        public bool ShowPowerDensityLoading { 
            get => this._showPowerDensityLoading;
            set => SetProperty(ref this._showPowerDensityLoading, value);
        }

        public PowerDensity CurrentPowerDensity { 
            get => this._selectedPowerDensity;
            set => SetProperty(ref this._selectedPowerDensity, value);
        }

        private async Task CommitHandler() {
            var entity=this._context.Update(this.CurrentPowerDensity).Entity;
            if (entity != null) {
                //this._eventAggregator.GetEvent<RefreshDataEvent>().Publish();
                try {
                    var count=this._context.SaveChanges();
                    if (count > 0) {
                        this.MessageService.ShowMessage("Changes Saved.. Reloading");
                        await this.LoadAsync();
                        this._loaded = false;
                    } else {
                        this.MessageService.ShowMessage("Error: Save Failed,Count<0");
                    }
                } catch {
                    this.MessageService.ShowMessage("Error: Save Failed,Exception Thrown");
                }


            } else {
                this.MessageService.ShowMessage("Error: Save Failed");
            }

        }

        private async Task CancelEditHandler() {
            this._loaded = false;
            await this.LoadAsync();
        }
        
        //private async Task SaveHandler() {

        //}

        //private async Task CancelHandler() {

        //}

        private async Task LoadAsync() {
            await Task.Run(() => {
                if (!this._loaded) {
                    this.DispatcherService.BeginInvoke(() => this.ShowPowerDensityLoading = true);
                    this._context.PowerDensities.Include(e=>e.Led).Load();
                    var powerDensities = this._context.PowerDensities.Include(e => e.Led).ToList();
                    this.DispatcherService.BeginInvoke(() => {
                        this.PowerDensities = new ObservableCollection<PowerDensity>(powerDensities);
                    });
                    this._loaded = true;
                    this.DispatcherService.BeginInvoke(() => this.ShowPowerDensityLoading = false);
                }
                
            });
        }
    }
}
