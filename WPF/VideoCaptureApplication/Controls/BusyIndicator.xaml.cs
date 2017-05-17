using System.Windows;
using System.Windows.Controls;

namespace VideoCaptureApplication.Controls
{
    /// <summary>
    /// Logique d'interaction pour BusyIndicator.xaml
    /// </summary>
    public partial class BusyIndicator : UserControl
    {
        public static readonly DependencyProperty LoaderTextProperty = DependencyProperty.Register("LoaderText", typeof(string), typeof(BusyIndicator), new PropertyMetadata(null));

        public string LoaderText
        {
            get { return (string)GetValue(LoaderTextProperty); }
            set { SetValue(LoaderTextProperty, value); }
        }

        public BusyIndicator()
        {
            InitializeComponent();
            this.DataContext = this;

        }

        /// <summary>
        /// Handles the Loaded event of the SplashWindow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void BusyIndicator_Loaded(object sender, RoutedEventArgs e)
        {
            SetPosition();
        }

        /// <summary>
        /// Sets the position.
        /// </summary>
        public void SetPosition()
        {
            double leftPos = (this.LayoutRoot.ActualWidth / 2) - (Splash.ActualWidth / 2);
            double topPos = (this.LayoutRoot.ActualHeight / 2) - (Splash.ActualHeight / 2);
            Canvas.SetLeft(this.Splash, leftPos);
            Canvas.SetTop(this.Splash, topPos);
        }
    }
}
