using DataAccess.Repository.IRepository;
using DataModels.AdminSideViewModels;
using HelloDoc;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections;

namespace DataAccess.Repository
{
    public class RequestRepository : Repository<Request>, IRequestRepository
    {
        public HelloDocDbContext _db;
        public RequestRepository(HelloDocDbContext db) : base(db)
        {
            _db = db;
        }

        public int Countbystate(string state)
        {
            if (state == "New")
            {
                return CountbyStatus(1);
            }
            else if (state == "Pending")
            {
                return CountbyStatus(2);
            }
            else if (state == "Active")
            {
                return CountbyStatus(4) + CountbyStatus(5);
            }
            else if (state == "Conclude")
            {
                return CountbyStatus(6);
            }
            else if (state == "Toclose")
            {
                return CountbyStatus(3) + CountbyStatus(7) + CountbyStatus(8);
            }
            else if (state == "Unpaid")
            {
                return CountbyStatus(9);
            }
            else
            {
                return CountbyStatus(10);
            }
        }

        public int CountbyStatus(int status)
        {
            return _db.Requests.Where(r => r.Status == status).Count();
        }


        public List<Request> GetRequestsbyState(string state)
        {
            if (state == "New")
            {
                return GetRequestsbyStatus(1);
            }
            else if (state == "Pending")
            {
                return GetRequestsbyStatus(2);
            }
            else if (state == "Active")
            {
                return GetRequestsbyStatus(4).Concat(GetRequestsbyStatus(5)).ToList();
            }
            else if (state == "Conclude")
            {
                return GetRequestsbyStatus(6);
            }
            else if (state == "Toclose")
            {
                return GetRequestsbyStatus(3).Concat(GetRequestsbyStatus(7)).Concat(GetRequestsbyStatus(8)).ToList();
            }
            else if (state == "Unpaid")
            {
                return GetRequestsbyStatus(9);
            }
            else if (state == "all")
            {
                return _db.Requests
                    .Include(r => r.User)
                    .Include(r => r.Requestclients)
                    .Include(r => r.Physician)
                    .Include(r => r.User.Region)
                    .Include(r => r.Requeststatuslogs)
                    .Include(x => x.Requestcloseds)
                    .Include(x => x.Requesttype)
                    .Include(r => r.Requestnotes)
                    .Include(x => x.Requestwisefiles)
                    .Include(x => x.Encounters)
                    .AsEnumerable()
                    .Where(x => x.Isdeleted == null || x.Isdeleted[0] == false)
                    .ToList();
            }
            else
            {
                return GetRequestsbyStatus(10);
            }
        }

        public List<Request> GetRequestsbyStatus(int status)
        {
            var request = _db.Requests
                .Include(r => r.User)
                .Include(r => r.Requestclients)
                .Include(r => r.Physician)
                .Include(r => r.User.Region)
                .Include(r => r.Requeststatuslogs)
                .Include(r => r.Requestnotes)
                .Include(x => x.Requesttype)
                .Include(x => x.Requestwisefiles)
                .Include(x => x.Encounters)
                .Include(x => x.Requestcloseds).ToList();
            request = request.Where(x => x.Isdeleted == null || x.Isdeleted[0] == false).ToList();
            request = request.Where(x => x.Status == status).ToList();
            return request;
        }

        public string GetstatebyStatus(int status)
        {
            if (status == 1)
                return "New";
            else if (status == 2)
                return "Pending";
            else if (status == 5 || status == 4)
                return "Active";
            else if (status == 3 || status == 7 || status == 8)
                return "Toclose";
            else if (status == 6)
                return "Conclude";
            else if (status == 9)
                return "Unpaid";
            else if (status == 10)
                return "Close";
            else
                return "Block";
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Request request)
        {
            _db.Requests.Update(request);
        }



        public List<SearchRecodsViewModel> GetFilterdData(string selectstatus, string patientname, int selecttype, string fromdate, string todate, string providername, string emailid, string mobile, int currentpage, int pagesize, bool order)
        {
            List<SearchRecodsViewModel> models = new List<SearchRecodsViewModel>();
            var requests = GetRequestsbyState(selectstatus);

            if (patientname != null)
            {
                patientname = patientname.Replace(" ", ""); patientname = patientname.ToLower();
                requests = requests.Where(x => (x.Requestclients.FirstOrDefault().Firstname + x.Requestclients.FirstOrDefault().Lastname).ToLower().Contains(patientname)).ToList();
            }
            if (selecttype != 0)
            {
                requests = requests.Where(x => x.Requesttypeid == selecttype).ToList();
            }
            if (fromdate != null)
            {
                DateTime FromDate = DateTime.Parse(fromdate);
                requests = requests.Where(x => x.Createddate >= FromDate).ToList();
            }
            if (todate != null)
            {
                DateTime ToDate = DateTime.Parse(todate);
                requests = requests.Where(x => x.Createddate <= ToDate.AddDays(1)).ToList();
            }
            if (providername != null)
            {
                providername = providername.Replace(" ", ""); providername = providername.ToLower();
                requests = requests.Where(x => x.Physician != null).ToList();
                requests = requests.Where(x => (x.Physician.Firstname + x.Physician.Lastname).ToLower().Contains(providername)).ToList();
            }
            if (emailid != null)
            {
                emailid = emailid.Replace(" ", ""); emailid = emailid.ToLower();
                requests = requests.Where(x => x.Requestclients.FirstOrDefault().Email.ToLower().Contains(emailid)).ToList();
            }
            if (mobile != null)
            {
                mobile = mobile.Replace(" ", ""); mobile = mobile.ToLower();
                requests = requests.Where(x => x.Requestclients.FirstOrDefault().Phonenumber != null).ToList();
                requests = requests.Where(x => x.Requestclients.FirstOrDefault().Phonenumber.Contains(mobile)).ToList();
            }


            if (order)
            {
                requests = requests
    .OrderBy(x => x.Requeststatuslogs.Any(z => z.Status == 9) ? x.Requeststatuslogs.Where(z => z.Status == 9).Min(log => log.Createddate) : DateTime.MinValue)
    .ToList();

            }
            else
            {
                requests = requests
    .OrderByDescending(x => x.Requeststatuslogs.Any(z => z.Status == 9) ? x.Requeststatuslogs.Where(z => z.Status == 9).Min(log => log.Createddate) : DateTime.MinValue)
    .ToList();

            }
            if (currentpage != 0)
            {
                requests = requests.Skip((currentpage - 1) * pagesize).Take(pagesize).ToList();
            }

            foreach (var item in requests)
            {
                SearchRecodsViewModel search = new SearchRecodsViewModel();
                search.Requestid = item.Requestid;
                search.PatientName = item.Requestclients.FirstOrDefault().Firstname + " " + item.Requestclients.FirstOrDefault().Lastname;
                //search.Requestor = item.Firstname + " " + item.Lastname;
                search.Requestor = item.Requesttype.Name;
                search.DateofService = item.Createddate;
                var lastLog = item.Requeststatuslogs.Where(x => x.Status == 3 || x.Status == 7 || x.Status == 8).LastOrDefault();
                search.CloseCaseDate = lastLog != null ? lastLog.Createddate : null;
                search.Email = item.Requestclients.FirstOrDefault().Email;
                search.Mobile = item.Requestclients.FirstOrDefault().Phonenumber;
                search.Address = item.Requestclients.FirstOrDefault().Address;
                search.Zip = item.Requestclients.FirstOrDefault().Zipcode;
                if (item.Status == 1) search.Status = "New";
                if (item.Status == 2) search.Status = "Pending";
                if (item.Status == 4 || item.Status == 5) search.Status = "Active";
                if (item.Status == 6) search.Status = "Conclude";
                if (item.Status == 3 || item.Status == 7 || item.Status == 8) search.Status = "ToClose";
                if (item.Status == 9) search.Status = "Unpaid";
                if (item.Physician != null)
                {
                    search.Phyicianname = item.Physician.Firstname + " " + item.Physician.Lastname;
                }
                else
                {
                    search.Phyicianname = "-";
                }
                var PhysicianNote = item.Requestnotes.LastOrDefault();
                search.PhysicianNote = PhysicianNote != null ? PhysicianNote.Physiciannotes : "-";
                var notelog = item.Requeststatuslogs.Where(x => x.Physicianid != null).LastOrDefault();
                search.CancelledByPhyNote = notelog != null ? notelog.Notes : "-";
                var AdminNote = item.Requestnotes.LastOrDefault();
                search.AdminNote = AdminNote != null ? AdminNote.Adminnotes : "-";
                search.PatientNote = item.Requestclients.Last().Notes;
                models.Add(search);
            }

            return models;
        }

        public int SearchRecordsCount(string selectstatus, string patientname, int selecttype, string fromdate, string todate, string providername, string emailid, string mobile)
        {
            List<SearchRecodsViewModel> models = new List<SearchRecodsViewModel>();
            var requests = GetRequestsbyState(selectstatus);

            if (patientname != null)
            {
                patientname = patientname.Replace(" ", ""); patientname = patientname.ToLower();
                requests = requests.Where(x => (x.Requestclients.FirstOrDefault().Firstname + x.Requestclients.FirstOrDefault().Lastname).ToLower().Contains(patientname)).ToList();
            }
            if (selecttype != 0)
            {
                requests = requests.Where(x => x.Requesttypeid == selecttype).ToList();
            }
            if (fromdate != null)
            {
                DateTime FromDate = DateTime.Parse(fromdate);
                requests = requests.Where(x => x.Createddate >= FromDate).ToList();
            }
            if (todate != null)
            {
                DateTime ToDate = DateTime.Parse(todate);
                requests = requests.Where(x => x.Createddate <= ToDate).ToList();
            }
            if (providername != null)
            {
                providername = providername.Replace(" ", ""); providername = providername.ToLower();
                requests = requests.Where(x => x.Physician != null).ToList();
                requests = requests.Where(x => (x.Physician.Firstname + x.Physician.Lastname).ToLower().Contains(providername)).ToList();
            }
            if (emailid != null)
            {
                emailid = emailid.Replace(" ", ""); emailid = emailid.ToLower();
                requests = requests.Where(x => x.Requestclients.FirstOrDefault().Email.ToLower().Contains(emailid)).ToList();
            }
            if (mobile != null)
            {
                mobile = mobile.Replace(" ", ""); mobile = mobile.ToLower();
                requests = requests.Where(x => x.Requestclients.FirstOrDefault().Phonenumber != null).ToList();
                requests = requests.Where(x => x.Requestclients.FirstOrDefault().Phonenumber.Contains(mobile)).ToList();
            }

            return requests.Count();
        }

        public byte[] GetFilterdDatatoexcle(string selectstatus, string patientname, int selecttype, string fromdate, string todate, string providername, string emailid, string mobile, bool order)
        {
            var model = GetFilterdData(selectstatus, patientname, selecttype, fromdate, todate, providername, emailid, mobile, 0, 0, order);
            using (var workbook = new XSSFWorkbook())
            {
                ISheet sheet = workbook.CreateSheet("FilteredRecord");
                IRow headerRow = sheet.CreateRow(0);
                headerRow.CreateCell(0).SetCellValue("Sr No.");
                headerRow.CreateCell(1).SetCellValue("Request Id");
                headerRow.CreateCell(2).SetCellValue("Patient Name");
                headerRow.CreateCell(3).SetCellValue("Requestor");
                headerRow.CreateCell(4).SetCellValue("Date Of Service");
                headerRow.CreateCell(5).SetCellValue("Close Case Date");
                headerRow.CreateCell(6).SetCellValue("Email");
                headerRow.CreateCell(7).SetCellValue("Phone Number");
                headerRow.CreateCell(8).SetCellValue("Address");
                headerRow.CreateCell(9).SetCellValue("Zip");
                headerRow.CreateCell(10).SetCellValue("RequestStatus");
                headerRow.CreateCell(11).SetCellValue("Physician");
                headerRow.CreateCell(12).SetCellValue("Physician Note");
                headerRow.CreateCell(13).SetCellValue("Cancellled By Physician Note");
                headerRow.CreateCell(14).SetCellValue("Admin Note");


                for (int i = 0; i < model.Count; i++)
                {
                    IRow row = sheet.CreateRow(i + 1);
                    row.CreateCell(0).SetCellValue(i + 1);
                    row.CreateCell(1).SetCellValue(model[i].Requestid);
                    row.CreateCell(2).SetCellValue(model[i].Requestor);
                    row.CreateCell(3).SetCellValue(model[i].PatientName);
                    row.CreateCell(4).SetCellValue(model[i].DateofService.ToString("MMM dd,yyyy"));
                    if (model[i].CloseCaseDate != null)
                    {
                        row.CreateCell(5).SetCellValue(model[i].CloseCaseDate.Value.ToString("MMM dd,yyyy"));
                    }
                    else
                    {
                        row.CreateCell(5).SetCellValue("-");
                    }
                    row.CreateCell(6).SetCellValue(model[i].Email);
                    row.CreateCell(7).SetCellValue(model[i].Mobile);
                    row.CreateCell(8).SetCellValue(model[i].Address);
                    row.CreateCell(9).SetCellValue(model[i].Zip);
                    row.CreateCell(10).SetCellValue(model[i].Status);
                    row.CreateCell(11).SetCellValue(model[i].Phyicianname);
                    row.CreateCell(12).SetCellValue(model[i].PhysicianNote);
                    row.CreateCell(13).SetCellValue(model[i].CancelledByPhyNote);
                    row.CreateCell(14).SetCellValue(model[i].AdminNote);
                }

                using (var stream = new MemoryStream())
                {
                    workbook.Write(stream);
                    var content = stream.ToArray();
                    return content;
                }
            }
        }

        public void DeleteThisRequest(int requestid)
        {
            var request = GetFirstOrDefault(x => x.Requestid == requestid);
            BitArray fortrue = new BitArray(1);
            fortrue[0] = true;
            request.Isdeleted = fortrue;
            Update(request);
            Save();
        }

        public int Countbystateforprovider(string state, int providerid)
        {
            var count = 0;
            if (state == "New")
            {
                count = GetRequestsbyState("Pending").Where(x => (x.Physicianid == providerid && x.Accepteddate == null)).Count();
            }
            if (state == "Pending")
            {
                count = GetRequestsbyState("Pending").Where(x => (x.Physicianid == providerid && x.Accepteddate != null)).Count();
            }
            if (state == "Active")
            {
                count = GetRequestsbyState("Active").Where(x => x.Physicianid == providerid).Count();
            }
            if (state == "Conclude")
            {
                count = GetRequestsbyState("Conclude").Where(x => x.Physicianid == providerid).Count();
            }
            return count;
        }

        public Request GetById(int id)
        {
            var request = _db.Requests
                .Include(r => r.User)
                .Include(r => r.Requestclients)
                .Include(r => r.Physician)
                .Include(r => r.Requestnotes)
                .Include(r => r.User.Region)
                .Include(r => r.Requeststatuslogs)
                .Include(x => x.Requesttype)
                .Include(x => x.Requestwisefiles)
                .Include(x => x.Encounters)
                .Include(x => x.Requestcloseds).FirstOrDefault(x => x.Requestid == id);
            return request;
        }
    }
}
