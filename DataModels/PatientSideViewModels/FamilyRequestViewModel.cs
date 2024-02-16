using Microsoft.AspNetCore.Http;

namespace HelloDoc.Areas.Patient.ViewModels
{
    public class FamilyRequestViewModel
    {
        public string F_FirstName { get; set; }
        public string F_LastName { get; set; }
        public string F_Email { get; set; }
        public string F_Phone { get; set; }
        public string Relation { get; set; }

        public string Symptoms { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }
        public DateOnly BirthDate { get; set; }

        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Room { get; set; }

        public int Requesttypeid { get; set; } = 2;
        public short Status { get; set; } = 1;
        public List<IFormFile> Upload { get; set; }

    }

}
