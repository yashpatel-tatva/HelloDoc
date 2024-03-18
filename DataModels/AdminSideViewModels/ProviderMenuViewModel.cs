using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.AdminSideViewModels
{
    public class ProviderMenuViewModel
    {
        public int PhysicanId { get; set; }
        public string Name { get; set; }
        public bool StopNotification { get; set; }
        public string Role { get; set; }
        public string OnCallStatus { get; set; }
        public string Status { get; set; }
        public string Email { get; set; }
    }
}
