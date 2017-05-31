using SharpAvi;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

using VideoCaptureApplication.TestCapture;

namespace VideoCaptureApplication.Views
{
    /// <summary>
    /// Logique d'interaction pour VideoRecorderControl.xaml
    /// </summary>
    public partial class VideoRecorderControl : UserControl
    {
        private static readonly DependencyPropertyKey IsRecordingPropertyKey =
            DependencyProperty.RegisterReadOnly("IsRecording", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));
        public static readonly DependencyProperty IsRecordingProperty = IsRecordingPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey ElapsedPropertyKey =
            DependencyProperty.RegisterReadOnly("Elapsed", typeof(string), typeof(MainWindow), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty ElapsedProperty = ElapsedPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey HasLastScreencastPropertyKey =
            DependencyProperty.RegisterReadOnly("HasLastScreencast", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));
        public static readonly DependencyProperty HasLastScreencastProperty = HasLastScreencastPropertyKey.DependencyProperty;
        
        private string lastFileName;
        private VideoRecorder videoRecorder;
        private readonly Stopwatch recordingStopwatch = new Stopwatch();
        private string outputFolder;
        private FourCC encoder;
        private int encodingQuality;
        private bool minimizeOnStart;

        private readonly DispatcherTimer recordingTimer;

        public bool IsRecording{ get; set; }

        public string Elapsed{ get; set; }

        public bool HasLastScreencast{ get; set; }

        public MainWindow MasterWindow
        {
            get { return (MainWindow)Application.Current.MainWindow; }
        }

        public VideoRecorderControl()
        {
            InitializeComponent();
            /*Settings*/
            recordingTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            recordingTimer.Tick += recordingTimer_Tick;
            
            var exePath = new Uri(System.Reflection.Assembly.GetEntryAssembly().Location).LocalPath;
            outputFolder = Path.GetDirectoryName(exePath);
            encoder = KnownFourCCs.Codecs.MotionJpeg;
            //minimizeOnStart = true;
            encodingQuality = 70;

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
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
            }
        }

        private void btnInternet_Click(object sender, RoutedEventArgs e)
        {
            String Result = "Raté";
            /*
             * http://hubblesource.stsci.edu/sources/video/clips/details/images/hst_1.mpg
             */
            
            InputDialogSample inputDialog = new InputDialogSample();
            if (inputDialog.ShowDialog() == true)
                Result = inputDialog.Answer;
            btnInternet.Content = Result;
            
        }


        private void StartRecording()
        {
            var stopwatch = new Stopwatch();
            if (IsRecording)
                throw new InvalidOperationException("Already recording.");

            //if (minimizeOnStart)
            //    WindowState = WindowState.Minimized;

            Elapsed = "00:00";
            HasLastScreencast = false;
            IsRecording = true;

            recordingStopwatch.Reset();
            recordingTimer.Start();
            
            stopwatch.Start();

            lastFileName = Path.Combine(outputFolder, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".avi");
            videoRecorder = new VideoRecorder(lastFileName, encoder, encodingQuality);

            stopwatch.Stop();

            recordingStopwatch.Start();
        }

        private void StartReading()
        {
            FileStream fs = File.Open(lastFileName, FileMode.OpenOrCreate,FileAccess.Read,FileShare.ReadWrite);

            byte[] toSend = new byte[512];
            
            var retour = fs.ReadAsync(toSend, 0, 512);

        }

        private void StopRecording()
        {
            if (!IsRecording)
                throw new InvalidOperationException("Not recording.");

            videoRecorder.Dispose();
            videoRecorder = null;

            recordingTimer.Stop();
            recordingStopwatch.Stop();

            IsRecording = false;
            HasLastScreencast = true;
        }

        private void recordingTimer_Tick(object sender, EventArgs e)
        {
            var elapsed = recordingStopwatch.Elapsed;
            Elapsed = string.Format(
                "{0:00}:{1:00}",
                Math.Floor(elapsed.TotalMinutes),
                elapsed.Seconds);
        }

        private void btnRcrdStart_Click(object sender, RoutedEventArgs e)
        {
            StartRecording();
        }

        private void btnRcrdStop_Click(object sender, RoutedEventArgs e)
        {
            StopRecording();
        }

        private void btnReadStart_Click(object sender, RoutedEventArgs e)
        {
            StartReading();
        }
    }
}
