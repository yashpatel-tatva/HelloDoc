namespace DataModels.AdminSideViewModels
{
    public class CreateShiftViewModel
    {
        public int region { get; set; }
        public int physician { get; set; }
        public string shiftdate { get; set; }
        public string starttime { get; set; }
        public string endtime { get; set; }
        public bool repeatonoff { get; set; }
        public List<string> repeatdays { get; set; }
        public int repeattimes { get; set; }
        public string format { get; set; }
    }
}
