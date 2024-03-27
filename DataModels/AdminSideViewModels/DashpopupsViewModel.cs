using HelloDoc;

namespace DataModels.AdminSideViewModels
{
    public class DashpopupsViewModel
    {
        public int RequestId { get; set; }
        public int RequestType { get; set; }
        public string PatientEmail { get; set; }
        public string PatientMobile { get; set; }
        public string PatientName { get; set; }
        public List<Casetag> casetags { get; set; }
        public List<Region> regions { get; set; }
        public List<Physician> physicians { get; set; }

    }
}
