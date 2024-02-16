using HelloDoc.DataModels;
using Microsoft.AspNetCore.Http;

namespace HelloDoc.Areas.Patient.ViewModels
{
    public class checkbox
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class PatientDashboardViewModel
    {

        public enum Statuses
        {
            January,
            Unassigned,
            Accepted,
            Cancelled,
            MDEnRoute,
            MDOnSite,
            Closed,
            Clear,
            Unpaid
        }
        public User User { get; set; }
        public List<Request> Requests { get; set; }

        public string Statusname(int i)
        {
            string status = ((Statuses)i).ToString();
            return status;
        }
        public List<Requestwisefile> requestwisefiles { get; set; }

        public DateTime birthdate { get; set; }
        public int RequestsId { get; set; }
        public bool showdocument { get; set; }

        public int reqfileid { get; set; }
        public List<IFormFile> Upload { get; set; }
    }
}
