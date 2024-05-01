using Microsoft.AspNetCore.Mvc;

namespace DataModels.AdminSideViewModels
{
    public class SendOrderViewModel
    {
        public int RequestId { get; set; }

        [Remote(action: "VerifyProfession", controller: "AdminProfile", ErrorMessage = "Please Select State.")]
        public int ProfessionId { get; set; }

        [Remote(action: "VerifyVendor", controller: "AdminProfile", ErrorMessage = "Please Select State.")]
        public int VendorId { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string Prescription { get; set; }
        public int Refill { get; set; }

        public string CreatedBy { get; set; }

    }
}
