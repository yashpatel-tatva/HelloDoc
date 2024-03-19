using HelloDoc;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.AdminSideViewModels
{
    public class PhysicianAccountViewModel
    {
        public int PhysicianId { get; set; }
        public string Aspnetid { get; set; }    
        public string Username { get; set; }
        public string Password { get; set; }
        public int Status { get; set; }
        public int Roleid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        [MaybeNull]
        public string MedicalLicense { get; set; }
        [MaybeNull]
        public string NPINumber { get; set; }
        [MaybeNull]
        public string SynchronizationEmail { get; set; }
        public List<int> SelectedRegionCB { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public int RegionID { get; set; }
        public string Zip { get; set; }
        public string BusinessPhone { get; set; }
        public string BusinessName { get; set; }
        public string BusinessWebSite { get; set; }
        public string Photo { get; set; }
        public string Sign { get; set; }
        public string AdminNotes { get; set; }

        public bool IsAgreementDoc {get; set ; }
        public bool IsBackgroundDoc {get; set ; }
        public bool IsTrainingDoc {get; set ; }
        public bool IsNonDisclosureDoc {get; set ; }
        public bool IsLicenseDoc {get; set ; }
        public bool IsCredentialDoc {get; set; }


        public List<Region> regions { get; set; }

    }
}

