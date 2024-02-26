using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.AdminSideViewModels
{
    public class RequestNotesViewModel
    {
        public int RequestId { get; set; }
        public string TransferNotes { get; set; }
        public string PhysicianNotes { get; set; }
        public string AdminNotes { get; set; }
        public string AdminCancellationNotes { get; set; }
        public string PatientCancellationNotes { get; set; }
        public string PatientNotes { get; set;}
        public string PhysicianCancellationNotes { get; set; }

    }
}
