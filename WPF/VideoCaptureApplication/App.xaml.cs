
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using VideoCaptureApplication.Models;
using VideoCaptureApplication.Utils.Constants;
using VideoCaptureApplication.Utils.Helpers;

namespace VideoCaptureApplication
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            string dataPath = $@"{StringUtils.GetAppRootDirectory}\{AppConstants.FilesFolder}\";
            Parameter param = JsonUtils.ReadJsonFile(dataPath, typeof(Parameter)) as Parameter;

            string code = "fr-FR";

            if (param != null)
            {
                code = param.CurrentLanguage.Code;
            }

            Thread.CurrentThread.CurrentCulture = new CultureInfo(code);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(code);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            this.Dispatcher.UnhandledException += App_DispatcherUnhandledException;
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // TODO : Manage ex

            // Prevent default unhandled exception processing
            e.Handled = true;
        }
    }
}
