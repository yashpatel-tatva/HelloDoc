using HelloDoc;
using Microsoft.AspNetCore.Http;

namespace DataModels.AdminSideViewModels
{
    public class RequestViewUploadsViewModel
    {
        public int RequestsId { get; set; }
        public int reqfileid { get; set; }
        public string confirmation { get; set; }
        public string patientname { get; set; }
        public List<IFormFile> Upload { get; set; }
        public List<Requestwisefile> requestwisefiles { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string ProviderNote { get; set; }
        public DateTime PatientDOB { get; set; }
        public string PatientEmail { get; set; }
        public string PatientMobile { get; set; }
        public string role { get; set; }
    }
}
