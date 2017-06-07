using System;
using System.Windows;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Entity;


namespace VideoCaptureApplication.Views
{
    /// Logique d'interaction pour AuthenticationWindow.xaml
    public partial class AuthenticationWindow : Window
    {
        public AuthenticationWindow()
        {
            InitializeComponent();
        }

        private async void BtnAuthOk_Click(object sender, RoutedEventArgs e)
        {
            ////Initialize HTTP Client 
            HttpClient httpClient = new HttpClient();
            try
            {
                string resourceAddress = "http://localhost:63315/api/user/GetUsers/";
                USER user = new USER { USER_LOG = this.LoginTextBox.Text, USER_PWD = this.PwdTextBox.Password};
                httpClient.Timeout = TimeSpan.FromMilliseconds(100000);
                httpClient.DefaultRequestHeaders.Add("ContentType", "application/json; charser = utf-8");
                HttpResponseMessage wcfResponse = await httpClient.PostAsJsonAsync(resourceAddress, user);
                if (wcfResponse.IsSuccessStatusCode)
                {
                    var Respstring = await wcfResponse.Content.ReadAsStringAsync();
                    long userId = long.Parse(Respstring);
                    if (userId == 0)
                    {
                        MessageBox.Show("Votre login ou votre mot de passe n'est pas correct !!");
                    }
                    else
                    {
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Error Code" +
                    wcfResponse.StatusCode + " : Message - " + wcfResponse.ReasonPhrase);
                }
            }
            catch (HttpRequestException hre)
            {
                MessageBox.Show("Error:" + hre.Message);
            }
            catch (TaskCanceledException)
            {
                MessageBox.Show("Request canceled.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (httpClient != null)
                {
                    httpClient.Dispose();
                    httpClient = null;
                }
            }            
        }
    }
}
