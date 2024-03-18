using HelloDoc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DataModels.AdminSideViewModels
{
    public class AdminProfileViewModel
    {
        public string Aspnetid { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }
        public string Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        [Compare("Email" , ErrorMessage ="Email not match")]
        public string ConfirmEmail { get; set; }
        public string Mobile { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        [MaybeNull]
        public string City { get; set; }
        public int Region { get; set; }
        public List<Region> State { get; set; }
        public string Zip { get; set; }
        [MaybeNull]
        public string BillMobile { get; set; }
        public List<string> SelectRegion { get; set; }
    }
}
