using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.AdminSideViewModels
{
    public class RequestDataViewModel
    {
        public int RequestId { get; set; }
        public enum Requestby
        {
            first,
            Patient,
            FriendorFamily,
            Concierge,
            BusinessPartner
        }
        public string RequestTypeName(int by)
        {
            string By = ((Requestby)by).ToString();
            return By;
        }
        public int RequesttypeID { get; set; } 
        public string Notes { get; set; }
        public string ConfirmationNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime PatientDOB { get; set;}
        public string PatientMobile { get; set; }
        public string PatientEmail { get; set; }
        public string Region { get; set; }
        public string BuisnessName { get; set; }
        public string Room { get; set; }
    }
}
