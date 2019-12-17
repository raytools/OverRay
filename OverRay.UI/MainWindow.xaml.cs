using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace OverRay.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel vm)
        {
            InitializeComponent();
            DataContext = ViewModel = vm;

            ((INotifyCollectionChanged)LogBox.Items).CollectionChanged += OnLogCollectionChanged;
        }

        private void OnLogCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (VisualTreeHelper.GetChildrenCount(LogBox) > 0)
            {
                Border border = (Border)VisualTreeHelper.GetChild(LogBox, 0);
                ScrollViewer scrollViewer = (ScrollViewer)VisualTreeHelper.GetChild(border, 0);
                scrollViewer.ScrollToBottom();
            }
        }

        private MainViewModel ViewModel { get; }

        private void LogItemDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBoxItem item)
            {
                if (item.Content is string s)
                {
                    Clipboard.SetText(s);
                }
            }
        }
    }
}
