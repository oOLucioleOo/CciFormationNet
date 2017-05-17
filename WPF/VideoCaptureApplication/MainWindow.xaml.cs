using System.Windows;
using MahApps.Metro.Controls;
using VideoCaptureApplication.Controls;

namespace VideoCaptureApplication
{
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void SetBusy(Visibility isVisible)
        {
            MainBusyIndicator.Visibility = isVisible;
            MainBusyIndicator.progressBar.IsIndeterminate = isVisible == Visibility.Visible ? true : false;
        }
    }
}
