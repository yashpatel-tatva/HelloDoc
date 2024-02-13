using HelloDoc.DataModels;

namespace HelloDoc.Areas.Patient.ViewModels
{
    public class PatientDashboard
    {
        public User User { get; set; }
        public List<Request> Requests { get; set; }
    }
}
