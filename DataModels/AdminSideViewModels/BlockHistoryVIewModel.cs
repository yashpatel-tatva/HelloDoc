namespace DataModels.AdminSideViewModels
{
    public class BlockHistoryVIewModel
    {
        public int Blockid { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
