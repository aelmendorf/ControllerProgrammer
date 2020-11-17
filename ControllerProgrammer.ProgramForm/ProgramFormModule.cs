using ControllerProgrammer.ProgramForm.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using ControllerProgrammer.Common.Constants;

namespace ControllerProgrammer.ProgramForm {
    public class ProgramFormModule : IModule {
        public void OnInitialized(IContainerProvider containerProvider) {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(Regions.ProgramRegion, typeof(ProgramFormView));
            regionManager.RegisterViewWithRegion(Regions.DataInputRegion, typeof(DataInputView));
            regionManager.RegisterViewWithRegion(Regions.RecipeRegion, typeof(RecipeInputView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry) {

        }
    }
}