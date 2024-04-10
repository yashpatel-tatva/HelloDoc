namespace DataModels.AdminSideViewModels
{
    public class RequestNotesViewModel
    {
        public int RequestId { get; set; }
        public List<string> TransferNotes { get; set; }
        public string role { get; set; }
        public string PhysicianNotes { get; set; }
        public string AdminNotes { get; set; }
        public string AdminCancellationNotes { get; set; }
        public string PatientCancellationNotes { get; set; }
        public string PatientNotes { get; set; }
        public string PhysicianCancellationNotes { get; set; }

    }
}
