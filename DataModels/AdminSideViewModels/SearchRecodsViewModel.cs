namespace DataModels.AdminSideViewModels
{
    public class SearchRecodsViewModel
    {
        public int Requestid { get; set; }
        public string PatientName { get; set; }
        public string Requestor { get; set; }
        public DateTime DateofService { get; set; }
        public DateTime? CloseCaseDate { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string Zip { get; set; }
        public string Status { get; set; }
        public string Phyicianname { get; set; }
        public string PhysicianNote { get; set; }
        public string CancelledByPhyNote { get; set; }
        public string AdminNote { get; set; }
        public string PatientNote { get; set; }
    }
}
