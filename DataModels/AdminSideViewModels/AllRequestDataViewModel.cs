using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HelloDoc.Areas.PatientArea.ViewModels.PatientDashboardViewModel;

namespace DataModels.AdminSideViewModels
{
    public class AllRequestDataViewModel
    {
        public int RequestId { get; set; } 
        public string PatientName { get; set; }
        public DateOnly PatientDOB { get; set;}
        public string RequestorName { get; set; }
        public DateTime RequestedDate { get; set; }
        public string PatientPhone { get; set; }
        public string TransferNotes { get; set; }
        public string RequestorPhone { get; set; }
        public string RequestorEmail { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }
        public string ProviderEmail { get; set; }
        public string PatientEmail { get; set; }
        public int RequestType { get; set; }

        public string Region { get; set; }
        public string PhysicainName { get; set; }

        public enum Requestby
        {
            first,
            Patient,
            FriendorFamily,
            Concierge,
            BusinessPartner,
            VIP
        }
        public  string RequestTypeName(int by)
        {
            string By = ((Requestby)by).ToString();
            return By;
        }
    }
}
