using ControllerProgrammer.Common;
using Prism.Mvvm;

namespace ControllerProgrammer.Main.ViewModels {
    public class MainWindowViewModel : ProgrammerViewModelBase {
        private string _title = "Controller Programmer";

        public string Title {
            get => this._title;
            set => SetProperty(ref this._title, value);
        }

        public override bool KeepAlive => true;

        public MainWindowViewModel() {

        }
    }
}
