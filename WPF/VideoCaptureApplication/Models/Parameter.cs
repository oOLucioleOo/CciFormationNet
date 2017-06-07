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

        private double quality;
        public double Quality
        {
            get
            {
                return this.quality;
            }
            set
            {
                this.quality = value;
                this.RaisePropertyChanged();
            }
        }

        private double nbImage;
        public double NbImage
        {
            get
            {
                return this.nbImage;
            }
            set
            {
                this.nbImage = value;
                this.RaisePropertyChanged();
            }
        }
        private string login;
        public string Login
        {
            get
            {
                return this.login;
            }
            set
            {
                this.login = value;
                this.RaisePropertyChanged();
            }
        }
        private string pwd;
        public string Pwd
        {
            get
            {
                return this.pwd;
            }
            set
            {
                this.pwd = value;
                this.RaisePropertyChanged();
            }
        }
    }
}
