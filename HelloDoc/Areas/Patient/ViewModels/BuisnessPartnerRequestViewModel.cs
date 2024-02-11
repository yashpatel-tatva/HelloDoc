namespace HelloDoc.Areas.Patient.ViewModels
{
    public class BuisnessPartnerRequestViewModel
    {
        public string B_FirstName { get; set; }
        public string B_LastName { get; set; }
        public string B_Email { get; set; }
        public string B_Phone { get; set; }
        public string PropertyName { get; set; }
        public string CaseNumber {  get; set; }
        public string Symptopmps { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }
        public DateOnly BirthDate { get; set; }

        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Room { get; set; }

        public int Requesttypeid { get; set; } = 4;
        public short Status { get; set; } = 1;
    }
}
