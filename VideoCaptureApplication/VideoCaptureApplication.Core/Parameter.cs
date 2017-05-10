
using VideoCaptureApplication.Core.Base;

namespace VideoCaptureApplication.Core
{
    public class Parameter : BaseModel
    {  
        private Language currentLanguage;
        public Language CurrentLanguage
        {
            get
            {
                return this.currentLanguage;
            }
            set
            {
                this.currentLanguage = value;
                this.RaisePropertyChanged();
            }
        }
    }
}
