namespace DataModels.AdminSideViewModels
{
    public class ProviderOnCallViewModel
    {
        public DateTime datetoshow { get; set; }
        public int region { get; set; }
        public string showby { get; set; }
        public List<PhysicianData> physicianDatas { get; set; }
    }
}
