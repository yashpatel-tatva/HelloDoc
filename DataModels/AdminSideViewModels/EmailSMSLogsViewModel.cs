using System;
namespace DataModels.AdminSideViewModels
{
	public class EmailSMSLogsViewModel
	{
		public int id { get; set;}
		public string Recipient { get; set;	 }
		public string Action { get; set; }
		public string RoleName { get; set; }
		public string Mobile { get; set; }
		public string Email { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime SentDate { get; set; }
		public bool Sent { get; set; }
		public int Senttries { get; set; }
		public string ConfirmationNo { get; set; }
	}
}

