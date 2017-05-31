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
            //HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("http://localhost:63315");

            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //MessageBox.Show("1");
            //try
            //{
            //    var multipart = new MultipartFormDataContent();
            //    MessageBox.Show("2");
            //    multipart.Add(new StringContent(LoginTextBox.Text), "login");
            //    multipart.Add(new StringContent(PwdTextBox.Password), "password");
            //    MessageBox.Show("3");
            //    var response = await client.PostAsync("http://localhost:63315/api/user/GetUsers/", multipart);
            //    response.EnsureSuccessStatusCode(); // Throw on error code. 
            //    MessageBox.Show("Bonjour Esclave N°1239821381723");
            //    MessageBox.Show("4");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Utilisateur Inconnu");
            //}

            HttpClient httpClient = new HttpClient();
            try
            {
                string resourceAddress = "http://localhost:63315/api/user/GetUsers/";
                USER user = new USER { USER_LOG = this.LoginTextBox.Text, USER_PWD = this.PwdTextBox.Password};
                httpClient.Timeout = TimeSpan.FromMilliseconds(100000);
                httpClient.DefaultRequestHeaders.Add("ContentType", "application/json; charser = utf-8");
                HttpResponseMessage wcfResponse = await httpClient.PostAsJsonAsync(resourceAddress, user);
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

        private void BtnAuthExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public async Task<IApiResponse> ReadResponse(HttpResponseMessage message)
        {
            var response = new ApiResponse();
            response.statusCode = (int)message.StatusCode;
            if (response.StatusCode == 200)
                response.Data = await message.Content.ReadAsStringAsync();
            else
                response.Message = await message.Content.ReadAsStringAsync();
            return response;

        }
    }
}
