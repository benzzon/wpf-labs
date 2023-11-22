using LabsUI.Logging;
using LabsUI.ViewModels;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
//using YourApp.ViewModels;

namespace LabsUI
{
    public partial class MainWindow : Window
    {
        private readonly INLogService _logger;
        public MainWindow(INLogService nLogService)
        {
            InitializeComponent();
            _logger = nLogService;

            DataContext = new MainWindowViewModel(nLogService);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            LabsUI.Properties.Settings.Default.WinLeft = Application.Current.MainWindow.Left;
            LabsUI.Properties.Settings.Default.WinTop = Application.Current.MainWindow.Top;
            LabsUI.Properties.Settings.Default.WinHeight = Application.Current.MainWindow.Height;
            LabsUI.Properties.Settings.Default.WinWidth = Application.Current.MainWindow.Width;

            LabsUI.Properties.Settings.Default.Save();
        }
    }
}
