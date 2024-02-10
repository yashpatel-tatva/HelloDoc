namespace HelloDoc.Areas.Patient.ViewModels
{
    public class ConciergeRequestViewModel
    {
        public string C_FirstName { get; set; }
        public string C_LastName { get; set; }
        public string C_Email { get; set; }
        public string C_Phone { get; set; }
        public string HostelName{ get; set; }

        public string Symptopmps { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }
        public DateOnly BirthDate { get; set; }

        public string C_Street { get; set; }
        public string C_City { get; set; }
        public string C_State { get; set; }
        public string C_ZipCode { get; set; }
        public string Room { get; set; }

        public int Requesttypeid { get; set; } = 3;
        public short Status { get; set; } = 1;
    }
}
