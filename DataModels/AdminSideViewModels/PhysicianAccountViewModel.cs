using HelloDoc;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.AdminSideViewModels
{
    using Microsoft.AspNetCore.Http;
    using System.ComponentModel.DataAnnotations;

    public class PhysicianAccountViewModel
    {
        public int PhysicianId { get; set; }
        public string Aspnetid { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain alphanumeric characters and underscores.")]
        public string Username { get; set; }

        [Required]
        
        public string Password { get; set; }

        public int Status { get; set; }
        public int Roleid { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name can only contain letters.")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name can only contain letters.")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$", ErrorMessage = "Please enter valid phone number")]
        public string Phone { get; set; }

        [MaybeNull]
        public string MedicalLicense { get; set; }

        [MaybeNull]
        public string NPINumber { get; set; }

        [MaybeNull]
        [EmailAddress]
        public string SynchronizationEmail { get; set; }

        public List<int> SelectedRegionCB { get; set; }

        [Required]
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        [Required]
        public string City { get; set; }

        public int RegionID { get; set; }

        [Required]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "Zip code must be exactly 5 digits.")]
        public string Zip { get; set; }

        [Phone]
        public string BusinessPhone { get; set; }

        [Required]
        public string BusinessName { get; set; }

        [Url]
        public string BusinessWebSite { get; set; }

        public string Photo { get; set; }
        public string Sign { get; set; }
        [MaybeNull]
        public string AdminNotes { get; set; }

        public bool IsAgreementDoc { get; set; }
        public bool IsBackgroundDoc { get; set; }
        public bool IsTrainingDoc { get; set; }
        public bool IsNonDisclosureDoc { get; set; }
        public bool IsLicenseDoc { get; set; }
        public bool IsCredentialDoc { get; set; }

        public IFormFile AgreementDoc { get; set; }
        public IFormFile BackgroundDoc { get; set; }
        public IFormFile NonDisclosureDoc { get; set; }
        public IFormFile CredentialDoc { get; set; }


        public List<Region> regions { get; set; }


        public List<Role> roles { get; set; }



    }
}

