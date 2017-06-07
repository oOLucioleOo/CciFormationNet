using System.Windows;
using MahApps.Metro.Controls;
using VideoCaptureApplication.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;
using System.Windows.Forms;

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

            //if (Result == null) //user pressed cancel
            //    return null; 
        }
        
        private void AuthWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Views.AuthenticationWindow win2 = new Views.AuthenticationWindow();
            win2.Owner = System.Windows.Application.Current.MainWindow;
            win2.ShowDialog();
            //win2.ShowActivated = true;
            win2.Topmost = true;
        }
    }
}
