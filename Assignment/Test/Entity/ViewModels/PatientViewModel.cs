using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.ViewModels
{
    public class PatientViewModel
    {
        public int Id { get; set; }

        [RegularExpression(@"^[a-zA-Z]+(\s[a-zA-Z]+)*$", ErrorMessage = "Only alphabets and single space between words allowed")]
        public string FirstName { get; set; }
        [RegularExpression(@"^[a-zA-Z]+(\s[a-zA-Z]+)*$", ErrorMessage = "Only alphabets and single space between words allowed")]
        public string LastName { get; set; }
        public int DocterId { get; set; }

        public decimal Age { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [RegularExpression(@"^\+\d*$", ErrorMessage = "Phone number must start with + and contain only digits")]
        public string Phone { get; set; }
        public string Gender { get; set; }
        [RegularExpression(@"^[a-zA-Z]+(\s[a-zA-Z]+)*$", ErrorMessage = "Only alphabets and single space between words allowed")]
        public string Dieses { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]+(\s[a-zA-Z]+)*$", ErrorMessage = "Only alphabets and single space between words allowed")]
        public string Specialist { get; set; }
        public bool Isdeleted { get; set; }

    }
}
