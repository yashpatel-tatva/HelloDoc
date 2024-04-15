namespace DataModels.AdminSideViewModels
{
    public class SchedulingDataViewModel
    {
        public DateOnly firstMonthdate { get; set; }
        public DateOnly lastMonthdate { get; set; }
        public List<PhysicianData> physicianDatas { get; set; }
        public List<ShiftData> Shifts { get; set; }
        public List<int> WeekDates { get; set; }
    }
}
