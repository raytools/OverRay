using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace OverRay.UI.WPF
{
    public static class CommonCommands
    {
        /// <summary>
        /// Closes the current application
        /// </summary>
        public static ICommand CloseAppCommand => new RelayCommand(() => Application.Current.Shutdown());

        public static ICommand CloseWindowCommand => new RelayCommand(() => Application.Current.MainWindow.Close());

        public static ICommand OpenLinkCommand => new RelayCommand(link => Process.Start(new ProcessStartInfo((string)link)));
    }
}