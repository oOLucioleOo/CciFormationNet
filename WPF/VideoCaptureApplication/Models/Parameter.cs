using VideoCaptureApplication.Models.Base;

namespace VideoCaptureApplication.Models
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
