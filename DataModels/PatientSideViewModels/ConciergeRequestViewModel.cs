using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HelloDoc.Areas.PatientArea.ViewModels
{
    public class ConciergeRequestViewModel
    {
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter a valid")]
        public string C_FirstName { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter a valid")]
        public string C_LastName { get; set; }


        public string C_Email { get; set; }

        [StringLength(23)]
        [RegularExpression(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$", ErrorMessage = "Please enter valid phone number")]
        public string C_Phone { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter a valid")]
        [MaybeNull]
        public string HostelName{ get; set; }

        public string Symptoms { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter a valid")]
        public string FirstName { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter a valid")]
        public string LastName { get; set; }

        public string Email { get; set; }


        [StringLength(23)]
        [RegularExpression(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$", ErrorMessage = "Please enter valid phone number")]
        public string Phone { get; set; }


        public DateOnly BirthDate { get; set; }


      
        [MaybeNull]
        public string C_Street { get; set; }

        [RegularExpression(@"^(?=.*\S)[a-zA-Z\s.'-]+$", ErrorMessage = "Enter a valid")]
        [MaybeNull]
        public string C_City { get; set; }

        [RegularExpression(@"^(?=.*\S)[a-zA-Z\s.'-]+$", ErrorMessage = "Enter a valid")]
        [MaybeNull]
        public string C_State { get; set; }

        [StringLength(10, ErrorMessage = "Enter valid Zip Code")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Enter a valid 6-digit zip code")]
        [MaybeNull]
        public string C_ZipCode { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Enter a valid")]
        [MaybeNull]
        public string Room { get; set; }

        public int Requesttypeid { get; set; } = 3;
        public short Status { get; set; } = 1;
    }
}
