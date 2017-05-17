
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace VideoCaptureApplication.Views
{
    public class BaseControl : UserControl, INotifyPropertyChanged
    {
        public object GenericParameter { get; set; }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
