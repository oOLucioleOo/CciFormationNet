using System;
using System.Windows;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Entity;
using VideoCaptureApplication.Models;
using VideoCaptureApplication.Utils.Constants;
using VideoCaptureApplication.Utils.Helpers;

using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;



namespace VideoCaptureApplication.Views
{
    /// Logique d'interaction pour AuthenticationWindow.xaml
    public partial class AuthenticationWindow : Window
    {

        private Parameter currentParameter;
        private string dataPath = string.Empty;
        public Parameter CurrentParameter { get; set; }


        public AuthenticationWindow()
        {
            InitializeComponent();
        }




        private void AuthenticationWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            InitializeComponent();
            dataPath = $@"{StringUtils.GetAppRootDirectory}\{AppConstants.FilesFolder}\";
            this.DataContext = this;
            CurrentParameter = new Parameter();
            CurrentParameter = JsonUtils.ReadJsonFile(dataPath, typeof(Parameter)) as Parameter;
            if (CurrentParameter.Login != null)
            {
                CheckBoxSave.IsChecked = true;
                PwdTextBox.Password = CurrentParameter.Pwd;
            }
        }


        private async void BtnAuthOk_Click(object sender, RoutedEventArgs e)
        {
            ////Initialize HTTP Client 
            HttpClient httpClient = new HttpClient();
            try
            {
                CurrentParameter.Pwd = PwdTextBox.Password;
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
                        if (CheckBoxSave.IsChecked.Value == false)
                        {
                            CurrentParameter.Login = null;
                            CurrentParameter.Pwd = null;
                            PwdTextBox.Password = "";
                        }
                        SaveData();
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

        public MainWindow MasterWindow
        {
            get { return (MainWindow)Application.Current.MainWindow; }
        }

        private async Task SaveData()
        {
            try
            {
                MasterWindow.SetBusy(Visibility.Visible);

                await Task.Run(() =>
                {
                    if (CurrentParameter != null)
                    {
                        if (!Directory.Exists(dataPath))  //! = false
                        {
                            Directory.CreateDirectory(dataPath);
                        }

                        // serialize JSON directly to a file
                        using (StreamWriter file = File.CreateText($@"{dataPath}\{AppConstants.FileName}"))
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            serializer.Serialize(file, CurrentParameter);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                //TODO : use logger in real world
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                MasterWindow.SetBusy(Visibility.Hidden);
            }
        }


        private void BtnAuthExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
