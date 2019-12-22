using System;
using System.Windows.Data;
using System.Windows.Input;
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
        }

        public ICommand AttachCommand { get; }

        private HookManager HookManager { get; }
        public ObservableQueue<string> Messages => Log.Messages;

        public bool IsHookAttached { get; set; }

        public bool IsLogPaused
        {
            get => Log.IsPaused;
            set => Log.IsPaused = value;
        }

        private void AttachToProcess()
        {
            InjectorResponse response = HookManager.Inject();
            switch (response)
            {
                case InjectorResponse.Success:
                    IsHookAttached = true;
                    Log.Add("Attached to process");
                    break;
                case InjectorResponse.ProcessNotFound:
                    IsHookAttached = false;
                    Log.Add("Process not found");
                    break;
                case InjectorResponse.OtherError:
                    IsHookAttached = false;
                    Log.Add(HookManager.ExceptionMessage);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}