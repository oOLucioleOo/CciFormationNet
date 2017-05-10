using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Drawing;
using Microsoft.Expression.Encoder.ScreenCapture;
using System.Windows.Shapes;
using SharpAvi.Output;
using NAudio.Wave;
using SharpAvi.Codecs;

namespace WpfApp
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AviWriter writer;
        private IAviAudioStream audioStream;
        private WaveInEvent audioSource;

        public MainWindow()
        {
            InitializeComponent();
            RcrdLabel.Visibility = Visibility.Hidden;


        }

        

        private void rcrdBtnAudio_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                var exePath = new Uri(System.Reflection.Assembly.GetEntryAssembly().Location).LocalPath;
                string outputFolder = System.IO.Path.GetDirectoryName(exePath);
                String fileName = System.IO.Path.Combine(outputFolder, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".avi");

                writer = new AviWriter(fileName)
                {
                    FramesPerSecond = 30,
                };
                audioStream = writer.AddAudioStream(channelCount:1, samplesPerSecond: 44100, bitsPerSample: 16);
                // The recommended length of audio block (duration of a video frame)
                // may be computed for PCM as
                var audioByteRate = (audioStream.BitsPerSample / 8) * audioStream.ChannelCount * audioStream.SamplesPerSecond;
                var audioBlockSize = (int)(audioByteRate / writer.FramesPerSecond);
                var audioBuffer = new byte[audioBlockSize];

                while (!StopRcrdBtn.IsPressed)
                {
                    // Get the data
                    audioStream.WriteBlock(audioBuffer, 0, audioBuffer.Length);
                }
                // Create encoder
                var encoder = new Mp3AudioEncoderLame(
                    /* channelCount: */ 2,
                    /* samplesPerSecond: */ 44100,
                    /* outputBitRateKbps: */ 192
                );

                // Create stream
                var encodingStream = writer.AddEncodingAudioStream(encoder);
                // Encode and write data
                encodingStream.WriteBlock(audioBuffer, 0, audioBuffer.Length);


                //audioSource = new WaveInEvent
                //{
                //    DeviceNumber = 0,
                //    WaveFormat = new WaveFormat(8000, 16, 1),
                //    BufferMilliseconds = 100,
                //    NumberOfBuffers = 3,
                //};
                //audioSource.StartRecording();
                RcrdLabel.Visibility = Visibility.Visible;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void StopRcrdBtnClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                //audioSource.StopRecording();
                //writer.Close();

                RcrdLabel.Visibility = Visibility.Hidden;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
    
}





