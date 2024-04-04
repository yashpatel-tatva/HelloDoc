using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DataModels.AdminSideViewModels
{

    public class VendorDetailViewModel
    {
        public int id { get; set; }

        [RegularExpression(@"^[a-zA-Z]+(\s[a-zA-Z]+)*$", ErrorMessage = "Only alphabets and single space between words allowed")]
        public string Name { get; set; }

        [RegularExpression(@"^\d{1}$", ErrorMessage = "Select At least One")]
        public int Profession { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Fax number must be 10 digits")]
        public string FaxNumber { get; set; }

        [RegularExpression(@"^\+\d*$", ErrorMessage = "Phone number must start with + and contain only digits")]
        public string PhoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [RegularExpression(@"^(\+\d*|[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,})$", ErrorMessage = "Invalid Business Contact")]
        public string BusinessContact { get; set; }

        [Required(ErrorMessage = "Street cannot be empty")]
        public string Street { get; set; }

        [RegularExpression(@"^[a-zA-Z]+(\s[a-zA-Z]+)*$", ErrorMessage = "Only alphabets and single space between words allowed")]
        public string City { get; set; }

        [RegularExpression(@"^[a-zA-Z]+(\s[a-zA-Z]+)*$", ErrorMessage = "Only alphabets and single space between words allowed")]
        public string State { get; set; }

        [RegularExpression(@"^\d{6}$", ErrorMessage = "Zip code must be 6 digits")]
        public string Zip { get; set; }
    }

}
