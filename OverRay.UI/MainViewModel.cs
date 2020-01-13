using System.Windows.Data;
using System.Windows.Input;
using OverRay.UI.WPF;

namespace OverRay.UI
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            AttachCommand = new RelayCommand(AttachToProcess);

            Logger = new Logger(1000);
            BindingOperations.EnableCollectionSynchronization(Messages, this);

            HookManager = new HookManager("OverRay.Hook.dll", Logger, "Rayman2", "Rayman2.exe", "Rayman2.exe.noshim");
            AttachToProcess();
        }

        public ICommand AttachCommand { get; }

        private HookManager HookManager { get; }
        private Logger Logger { get; }
        public ObservableQueue<string> Messages => Logger.Messages;

        public bool IsLogPaused
        {
            get => Logger.IsPaused;
            set => Logger.IsPaused = value;
        }

        private void AttachToProcess()
        {
            HookManager.Inject();
        }
    }
}