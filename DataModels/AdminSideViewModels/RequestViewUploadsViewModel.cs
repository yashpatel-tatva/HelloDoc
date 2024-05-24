using HelloDoc;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

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
        [RegularExpression(@"^[a-zA-Z]+(\s[a-zA-Z]+)*$", ErrorMessage = "Only alphabets and single space between words allowed")]
        public string FirstName { get; set; }
        [RegularExpression(@"^[a-zA-Z]+(\s[a-zA-Z]+)*$", ErrorMessage = "Only alphabets and single space between words allowed")]
        public string LastName { get; set; }
        public bool Isencounterfinalized { get; set; }

        [RegularExpression(@"^[a-zA-Z]+(\s[a-zA-Z]+)*$", ErrorMessage = "Only alphabets and single space between words allowed")]
        public string ProviderNote { get; set; }
        public DateTime PatientDOB { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string PatientEmail { get; set; }
        [RegularExpression(@"^(\+\d{1,3}[- ]?)?\d{10}$", ErrorMessage = "Mobile number format is not valid")]
        public string PatientMobile { get; set; }
        public string role { get; set; }
    }
}
