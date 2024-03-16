using HelloDoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.AdminSideViewModels
{
    public class AdminDashboardViewModel
    {
        public Admin admin { get; set; }
        public int newrequest{ get; set; }
        public int pendingrequest{ get; set; }
        public int activerequest{ get; set; }
        public int concluderequest{ get; set; }
        public int tocloserequest{ get; set; }
        public int unpaidrequest{ get; set; }
    }
}
