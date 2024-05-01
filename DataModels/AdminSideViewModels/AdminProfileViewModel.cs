using HelloDoc;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DataModels.AdminSideViewModels
{
    public class AdminProfileViewModel
    {
        public string Aspnetid { get; set; }
        public int Adminid { get; set; }
        public string Username { get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Password must be at least 8 characters and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string Password { get; set; }
        public string Status { get; set; }

        [Remote(action: "VerifyRole", controller: "AdminProfile", ErrorMessage = "Please Select Role.")]
        public string Role { get; set; }

        [RegularExpression(@"^[a-zA-Z]+(\s[a-zA-Z]+)*$", ErrorMessage = "Only alphabets and single space between words allowed")]
        public string FirstName { get; set; }
        [RegularExpression(@"^[a-zA-Z]+(\s[a-zA-Z]+)*$", ErrorMessage = "Only alphabets and single space between words allowed")]
        public string LastName { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Compare("Email", ErrorMessage = "Email not match")]
        public string ConfirmEmail { get; set; }
        [RegularExpression(@"^\+\d*$", ErrorMessage = "Phone number must start with + and contain only digits")]
        public string Mobile { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        [MaybeNull]
        [RegularExpression(@"^[a-zA-Z]+(\s[a-zA-Z]+)*$", ErrorMessage = "Only alphabets and single space between words allowed")]
        public string City { get; set; }
        [Remote(action: "VerifyRegion", controller: "AdminProfile", ErrorMessage = "Please Select state.")]
        public int Region { get; set; }
        public List<Region> State { get; set; }


        [Required(ErrorMessage = "At least one region must be selected.")]
        public List<string> selectedregion { get; set; }
        public List<Role> roles { get; set; }
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Zip code must be 6 digits")]
        public string Zip { get; set; }
        [MaybeNull]
        [RegularExpression(@"^\+\d*$", ErrorMessage = "Phone number must start with + and contain only digits")]
        public string BillMobile { get; set; }
        public List<string> SelectRegion { get; set; }

        public bool others { get; set; }

    }
}
