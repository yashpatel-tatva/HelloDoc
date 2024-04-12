using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HelloDoc.Areas.PatientArea.ViewModels
{
    public class BusinessPartnerRequestViewModel
    {

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter a valid")]
        public string B_FirstName { get; set; }



        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter a valid")]
        public string B_LastName { get; set; }


        public string B_Email { get; set; }


        [StringLength(23)]
        [RegularExpression(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$", ErrorMessage = "Please enter valid phone number")]
        public string B_Phone { get; set; }

        public string PropertyName { get; set; }

        [MaybeNull]
        public string CaseNumber { get; set; }


        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter a valid")]
        [MaybeNull]
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
        public string Street { get; set; }


        [RegularExpression(@"^(?=.*\S)[a-zA-Z\s.'-]+$", ErrorMessage = "Enter a valid")]
        [MaybeNull]
        public string City { get; set; }


        [RegularExpression(@"^(?=.*\S)[a-zA-Z\s.'-]+$", ErrorMessage = "Enter a valid")]
        [MaybeNull]
        public string State { get; set; }


        [StringLength(10, ErrorMessage = "Enter valid Zip Code")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Enter a valid 6-digit zip code")]
        [MaybeNull]
        public string ZipCode { get; set; }


        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Enter a valid")]
        [MaybeNull]
        public string Room { get; set; }

        public int Requesttypeid { get; set; } = 4;
        public short Status { get; set; } = 1;
    }
}
