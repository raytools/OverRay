using System.Windows.Data;
using System.Windows.Input;
using OverRay.Hook.Types;
using OverRay.Hook.Utils;
using OverRay.UI.WPF;

namespace OverRay.UI
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            AttachCommand = new RelayCommand(AttachToProcess);

            HookManager = new HookManager("OverRay.Hook.dll", "Rayman2", "Rayman2.exe", "Rayman2.exe.noshim");
            BindingOperations.EnableCollectionSynchronization(Log.Messages, this);

            AttachToProcess();
        }

        public ICommand AttachCommand { get; }

        private HookManager HookManager { get; }
        public ObservableQueue<string> Messages => Log.Messages;

        public bool IsLogPaused
        {
            get => Log.IsPaused;
            set => Log.IsPaused = value;
        }

        private void AttachToProcess()
        {
            HookManager.Inject();
        }
    }
}