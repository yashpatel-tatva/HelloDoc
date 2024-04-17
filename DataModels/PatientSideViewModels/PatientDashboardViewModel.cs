using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HelloDoc.Areas.PatientArea.ViewModels
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
            first,
            Unassigned,         //New 
            Accepted,           //Pending
            Cancelled,          //To-close
            MDEnRoute,          //Active
            MDOnSite,           //Active
            Conclude,           //Conclude
            CancelledByPatient, //To-close
            Closed,             //To-close
            Unpaid,             //Unpaid
            Clear
        }
        public User User { get; set; }
        public List<Request> Requests { get; set; }

        public string Statusname(int i)
        {
            string status = ((Statuses)i).ToString();
            return status;
        }
        public List<Requestwisefile> requestwisefiles { get; set; }

        [Required]
        public DateTime birthdate { get; set; }
        public int RequestsId { get; set; }
        public bool showdocument { get; set; }

        public int reqfileid { get; set; }
        public List<IFormFile> Upload { get; set; }
        public string Uploader { get; set; }
    }
}
