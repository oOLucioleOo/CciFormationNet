using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using VideoCaptureApplication.Core;
using VideoCaptureApplication.Resources;
using VideoCaptureApplication.Utils.Constants;
using VideoCaptureApplication.Utils.Helpers;

namespace VideoCaptureApplication.Views
{
    /// <summary>
    /// Logique d'interaction pour ParametersControl.xaml
    /// </summary>
    public partial class ParametersControl : BaseControl
    {
        private string dataPath = string.Empty;

        private Parameter currentParameter;
        public Parameter CurrentParameter
        {
            get
            {
                return this.currentParameter;
            }
            set
            {
                this.currentParameter = value;
                base.RaisePropertyChanged();
            }
        }

        private ObservableCollection<Language> langList;
        public ObservableCollection<Language> LanguageList
        {
            get
            {
                return this.langList;
            }
            set
            {
                this.langList = value;
                base.RaisePropertyChanged();
            }
        }

        public MainWindow MasterWindow
        {
            get { return (MainWindow)Application.Current.MainWindow; }
        }

        public ParametersControl()
        {
            InitializeComponent();

            dataPath = $@"{StringUtils.GetAppRootDirectory}\{AppConstants.FilesFolder}\";

            this.DataContext = this;
        }

        #region Events

        private async void ParametersControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            PopulateLanguages();

            await LoadData();

            if (CurrentParameter == null)
            {
                CurrentParameter = new Parameter();
            }

            CurrentParameter.CurrentLanguage = LanguageList.SingleOrDefault(l => l.Code.Equals(CurrentParameter.CurrentLanguage.Code));
        }

        private void LanguagesListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CurrentParameter.CurrentLanguage != null)
            {
                ApplicationResources applicationResources = Application.Current.Resources["ApplicationResources"] as ApplicationResources;
                if (applicationResources != null)
                {
                    applicationResources.ChangeCulture(CurrentParameter.CurrentLanguage.Code);
                }
            }
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            await SaveData();
        }

        #endregion

        #region Methods

        private async Task LoadData()
        {
            try
            {
                MasterWindow.SetBusy(Visibility.Visible);

                await Task.Run(() =>
                {
                    if (!Directory.Exists(dataPath))
                    {
                        Directory.CreateDirectory(dataPath);
                    }

                    CurrentParameter = JsonUtils.ReadJsonFile(dataPath, typeof(Parameter)) as Parameter;
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

        private async Task SaveData()
        {
            try
            {
                MasterWindow.SetBusy(Visibility.Visible);

                await Task.Run(() =>
                {
                    if (CurrentParameter != null)
                    {
                        if (!Directory.Exists(dataPath))
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

        private void PopulateLanguages()
        {
            LanguageList = new ObservableCollection<Language>();
            LanguageList.Add(new Language { Code = "fr-FR", ImageUrl = "/VideoCaptureApplication;component/Assets/Images/Languages/fr-FR.png", Name = "French" });
            LanguageList.Add(new Language { Code = "en-GB", ImageUrl = "/VideoCaptureApplication;component/Assets/Images/Languages/en-GB.png", Name = "English" });
        }

        #endregion 
    }
}
