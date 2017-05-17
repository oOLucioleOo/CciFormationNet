using MahApps.Metro.Controls.Dialogs;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

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

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();

        }

        private void VideoRecorderControl_OnLoaded(object sender, RoutedEventArgs e)
        {

        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (mePlayer.Source != null)
            {
                if (mePlayer.NaturalDuration.HasTimeSpan)
                    lblStatus.Content = String.Format("{0} / {1}", mePlayer.Position.ToString(@"mm\:ss"), mePlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
            }
            else
                lblStatus.Content = "No file selected...";
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            mePlayer.Play();
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            mePlayer.Pause();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            mePlayer.Stop();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".mp4"; // Default file extension
            dlg.Filter = "Text documents (.mp4)|*.mp4"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                mePlayer.Source = new Uri(filename);
            }
        }

        private async void btnInternet_Click(object sender, RoutedEventArgs e)
        {
            Uri Result;
            /*
             * http://hubblesource.stsci.edu/sources/video/clips/details/images/hst_1.mpg
             */
            /*InputDialogSample inputDialog = new InputDialogSample();
            if (inputDialog.ShowDialog() == true)
                Result = inputDialog.Answer;
            btnInternet.Content = Result;*/
            
            Result = new Uri(await MasterWindow.ShowInputDialog(sender, e));
            mePlayer.Source = Result;
            
        }

        private void btnRcrdStart_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnRcrdStop_Click(object sender, RoutedEventArgs e)
        {

        }


    }



}
