using System;
using System.Windows;
using System.Net.Http;
using System.Net.Http.Headers;

namespace VideoCaptureApplication.Views
{
    /// Logique d'interaction pour AuthenticationWindow.xaml
    public partial class AuthenticationWindow : Window
    {
        public AuthenticationWindow()
        {
            InitializeComponent();
        }

        private void BtnAuthOk_Click(object sender, RoutedEventArgs e)
        {
            //Initialize HTTP Client 
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:63315");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                var multipart = new MultipartFormDataContent();

                multipart.Add(new StringContent(LoginTextBox.Text), "Login");
                multipart.Add(new StringContent(PwdTextBox.Password), "pwd");

                var response = client.PostAsync("/api/user/GetUsers", multipart).Result;
                response.EnsureSuccessStatusCode(); // Throw on error code. 
                MessageBox.Show("Bonjour Esclave N°1239821381723");
            }

            catch (Exception ex)
            {
                MessageBox.Show("Utilisateur Inconnu");
            }
        }

        private void BtnAuthExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
