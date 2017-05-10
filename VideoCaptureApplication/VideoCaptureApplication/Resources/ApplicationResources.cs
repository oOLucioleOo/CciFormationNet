
using System.Globalization;
using System.Threading;
using VideoCaptureApplication.Core.Base;

namespace VideoCaptureApplication.Resources
{
    public class ApplicationResources : BaseModel
    {
        /// The application strings
        /// </summary>
        private static readonly ApplicationStrings applicationStrings = new ApplicationStrings();

        /// <summary>
        /// Gets the tab view.
        /// </summary>
        public ApplicationStrings Strings
        {
            get { return applicationStrings; }
        }

        /// <summary>
        /// Changes the culture.
        /// </summary>
        /// <param name="culture">The culture.</param>
        public void ChangeCulture(string culture)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture); // 2 letters iso
            }

            RaisePropertyChanged(nameof(Strings));
        }
    }
}
