namespace DataModels.AdminSideViewModels
{
    public class SendOrderViewModel
    {
        public int RequestId { get; set; }

        public int ProfessionId { get; set; }
        public int VendorId { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string Prescription { get; set; }
        public int Refill { get; set; }

        public string CreatedBy { get; set; }

    }
}
