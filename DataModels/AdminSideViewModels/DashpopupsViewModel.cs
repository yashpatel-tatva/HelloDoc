using HelloDoc;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.AdminSideViewModels
{
    public class DashpopupsViewModel
    {   
        public int RequestId { get; set; }

        [NotNull]
        public string Blockreason { get; set; }

        public int CaseTagID { get; set; }
        public string Notes { get; set; }

        public int RegionId {  get; set; }
        public int PhysicianId {  get; set; }

        public string PatientName { get; set; }
        public List<Casetag> casetags { get; set; }
        public List<Region> regions { get; set; }
        public List<Physician> physicians { get; set; }

    }
}
