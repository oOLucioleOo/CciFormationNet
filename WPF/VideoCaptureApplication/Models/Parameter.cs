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
        public double Quality { get; set; }
        public double NbImage { get; set; }

        //private Video currentVideo;
        //public Video CurrentVideo
        //{
        //    get
        //    {
        //        return this.currentVideo;
        //    }
        //    set
        //    {
        //        this.currentVideo = value;
        //        this.RaisePropertyChanged();
        //    }
        //}
    }
}
