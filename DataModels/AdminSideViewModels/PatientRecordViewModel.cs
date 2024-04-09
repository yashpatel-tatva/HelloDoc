using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.AdminSideViewModels
{
    public class PatientRecordViewModel
    {
        public int RequestId { get; set; }
        public string RequestorName { get; set;}
        public DateTime Createddate { get; set; }
        public string Confirmation { get; set; }
        public string Providername { get; set; }
        public DateTime Conculdedate { get; set; }
        public string Status { get; set; }
        public bool IsFinalReport { get; set; }
        public int DocumentCount { get; set; }
    }
}
