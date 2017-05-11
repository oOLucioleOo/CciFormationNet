using System.Windows;
using System.Windows.Controls;

namespace VideoCaptureApplication.Views
{
    /// <summary>
    /// Logique d'interaction pour VideoRecorderControl.xaml
    /// </summary>
    public partial class VideoRecorderControl : UserControl
    {
        public MainWindow MasterWindow
        {
            get { return (MainWindow)Application.Current.MainWindow; }
        }

        public VideoRecorderControl()
        {
            InitializeComponent();
        }

        private void VideoRecorderControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
