using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Regions;
using Prism.Mvvm;
using DevExpress.Mvvm;


namespace ControllerProgrammer.Common {
    public abstract class ProgrammerViewModelBase : Prism.Mvvm.BindableBase, DevExpress.Mvvm.ISupportServices, IRegionMemberLifetime {
        public IServiceContainer _serviceContainer = null;
        IServiceContainer ISupportServices.ServiceContainer { get { return ServiceContainer; } }
        protected IServiceContainer ServiceContainer {
            get {
                if (this._serviceContainer == null) {
                    this._serviceContainer = new ServiceContainer(this);
                }
                return this._serviceContainer;
            }
        }

        public abstract bool KeepAlive { get; }
    }

    public abstract class ProgrammerViewModelNavigationBase : Prism.Mvvm.BindableBase, DevExpress.Mvvm.ISupportServices, INavigateAsync, INavigationAware, IRegionMemberLifetime {
        public IServiceContainer _serviceContainer = null;
        IServiceContainer ISupportServices.ServiceContainer { get { return ServiceContainer; } }
        protected IServiceContainer ServiceContainer {
            get {
                if (this._serviceContainer == null) {
                    this._serviceContainer = new ServiceContainer(this);
                }
                return this._serviceContainer;
            }
        }

        public abstract bool KeepAlive { get; }
        public abstract void OnNavigatedTo(NavigationContext navigationContext);
        public abstract bool IsNavigationTarget(NavigationContext navigationContext);
        public abstract void OnNavigatedFrom(NavigationContext navigationContext);
        public void RequestNavigate(Uri target, Action<NavigationResult> navigationCallback) => throw new NotImplementedException();
        public void RequestNavigate(Uri target, Action<NavigationResult> navigationCallback, NavigationParameters navigationParameters) => throw new NotImplementedException();
    }
}
