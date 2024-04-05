using HelloDoc;

namespace DataModels.AdminSideViewModels
{
    public class LocationData
    {
        public int Physicianid {get ; set ;}
        public string Name {get ; set ;}
        public string Photo {get ; set ;}
        public decimal? Lat {get ; set ;}
        public decimal? Long { get; set; }
    }
}