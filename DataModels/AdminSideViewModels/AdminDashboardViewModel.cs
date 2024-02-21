using HelloDoc.DataModels;
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
        public List<Request> requests { get; set; }
    }
}
