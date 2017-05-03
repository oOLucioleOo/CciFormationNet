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
        }

        

        private void rcrdBtnAudio_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                audioStream = writer.AddAudioStream(1, 44100, 16);
                audioStream.Name = "Voice";

                audioSource = new WaveInEvent();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
       
    }
    
}





