using Microsoft.AspNetCore.Http;

namespace HelloDoc.Areas.Patient.ViewModels
{
    public class PatientRequestViewModel
    {

        public string Symptoms { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        public string Room { get; set; }
        public int Requesttypeid { get; set; } = 1;
        public short Status { get; set; } = 1;
        public string Password { get; set; }

        public List<IFormFile> Upload { get; set; }
    }
}
