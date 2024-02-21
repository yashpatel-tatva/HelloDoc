using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HelloDoc.Areas.PatientArea.ViewModels
{
    public class FamilyRequestViewModel
    {
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter Valid Name.")]
        public string F_FirstName { get; set; }
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter Valid Name.")]
        public string F_LastName { get; set; }


        public string F_Email { get; set; }

        [StringLength(23)]
        [RegularExpression(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$", ErrorMessage = "Please enter valid phone number")]
        public string F_Phone { get; set; }


        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter a valid")]
        [MaybeNull]
        public string Relation { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter a valid")]
        [MaybeNull]
        public string Symptoms { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter Valid Name.")]
        public string FirstName { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter Valid Name.")]
        public string LastName { get; set; }

        public string Email { get; set; }


        [StringLength(23)]
        [RegularExpression(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$", ErrorMessage = "Please enter valid phone number")]
        public string Phone { get; set; }
        public DateOnly BirthDate { get; set; }



        [RegularExpression(@"^(?=.*\S)[a-zA-Z\s.'-]+$", ErrorMessage = "Enter a valid")]
        [MaybeNull] public string Street { get; set; }


        [RegularExpression(@"^(?=.*\S)[a-zA-Z\s.'-]+$", ErrorMessage = "Enter a valid")]
        [MaybeNull] public string City { get; set; }


        [RegularExpression(@"^(?=.*\S)[a-zA-Z\s.'-]+$", ErrorMessage = "Enter a valid")]
        [MaybeNull] public string State { get; set; }

        [StringLength(10, ErrorMessage = "Enter valid Zip Code")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Enter a valid 6-digit zip code")]
        [MaybeNull]
        public string ZipCode { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Enter a valid")]
        [MaybeNull]
        public string Room { get; set; }

        public int Requesttypeid { get; set; } = 2;
        public short Status { get; set; } = 1;
        public List<IFormFile> Upload { get; set; }

    }

}
