using System.Windows;
using MahApps.Metro.Controls;
using VideoCaptureApplication.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using System;
using System.ComponentModel;
using System.IO;

namespace VideoCaptureApplication
{
    public partial class MainWindow : MetroWindow
    {
        //public string Result { get; set; }
        public MainWindow()
        {
            InitializeComponent();
    
        }

        public void SetBusy(Visibility isVisible)
        {
            MainBusyIndicator.Visibility = isVisible;
            MainBusyIndicator.progressBar.IsIndeterminate = isVisible == Visibility.Visible ? true : false;
        }

        public async Task<string> ShowInputDialog(object sender, RoutedEventArgs e)
        {
             return await this.ShowInputAsync("Lecture vidéo par URL", "Veuillez saisir le lien de la vidéo :");
        }
        
        private void AuthWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var exePath = new Uri(System.Reflection.Assembly.GetEntryAssembly().Location).LocalPath;
            string path = Path.GetDirectoryName(exePath);
            path += "\\stream";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            //Views.AuthenticationWindow win2 = new Views.AuthenticationWindow();
            //win2.Owner = System.Windows.Application.Current.MainWindow;
            //win2.ShowDialog();
            //win2.Topmost = true;
        }
    }
}
