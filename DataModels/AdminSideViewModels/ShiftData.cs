namespace DataModels.AdminSideViewModels
{
    public class ShiftData
    {
        public int ShiftId { get; set; }
        public string Location { get; set; }
        public string Regionname { get; set; }
        public string Description { get; set; }
        public DateOnly Shiftdate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Physicianid { get; set; }
        public string Physicianname { get; set; }
        public string cssClass { get; set; }
        public string format { get; set; }
        public int Status { get; set; }

        public string loggedrole { get; set; }
    }
}