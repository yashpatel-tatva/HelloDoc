﻿using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository.IServiceRepository;
using DataModels.AdminSideViewModels;
using HelloDoc;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.VisualBasic;
using static iTextSharp.text.pdf.AcroFields;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataAccess.ServiceRepository
{
    public class InvoiceRepository : IInvoiceReposiitory
    {
        private readonly ISchedulingRepository _scheduling;
        private readonly HelloDocDbContext _db;
        private readonly IAdminRepository _admin;
        private readonly IPhysicianRepository _physician;
        public InvoiceRepository(HelloDocDbContext helloDocDbContext, ISchedulingRepository schedulingRepository, IAdminRepository adminRepository, IPhysicianRepository physicianRepository)
        {
            _db = helloDocDbContext;
            _scheduling = schedulingRepository;
            _admin = adminRepository;
            _physician = physicianRepository;
        }
        public void Editnightshiftpayrate(int physicianid, int rate)
        {
            var payrate = _db.Payrates.FirstOrDefault(x => x.Physicinaid == physicianid);
            payrate.Nightshift = rate;
            _db.Payrates.Update(payrate);
            _db.SaveChanges();
        }
        public void Editshiftpayrate(int physicianid, int rate)
        {
            var payrate = _db.Payrates.FirstOrDefault(x => x.Physicinaid == physicianid);
            payrate.Shift = rate;
            _db.Payrates.Update(payrate);
            _db.SaveChanges();
        }
        public void Editnighthousecallpayrate(int physicianid, int rate)
        {
            var payrate = _db.Payrates.FirstOrDefault(x => x.Physicinaid == physicianid);
            payrate.Nighthousecall = rate;
            _db.Payrates.Update(payrate);
            _db.SaveChanges();
        }
        public void Editconsultpayrate(int physicianid, int rate)
        {
            var payrate = _db.Payrates.FirstOrDefault(x => x.Physicinaid == physicianid);
            payrate.Consult = rate;
            _db.Payrates.Update(payrate);
            _db.SaveChanges();
        }
        public void Editnightconsultpayrate(int physicianid, int rate)
        {
            var payrate = _db.Payrates.FirstOrDefault(x => x.Physicinaid == physicianid);
            payrate.Nightconsult = rate;
            _db.Payrates.Update(payrate);
            _db.SaveChanges();
        }
        public void Editbatchtestingpayrate(int physicianid, int rate)
        {
            var payrate = _db.Payrates.FirstOrDefault(x => x.Physicinaid == physicianid);
            payrate.Batchtesting = rate;
            _db.Payrates.Update(payrate);
            _db.SaveChanges();
        }
        public void Edithousecallpayrate(int physicianid, int rate)
        {
            var payrate = _db.Payrates.FirstOrDefault(x => x.Physicinaid == physicianid);
            payrate.Housecall = rate;
            _db.Payrates.Update(payrate);
            _db.SaveChanges();
        }

        public BiWeekViewModel BiWeekData(int physicianid, DateTime date)
        {
            var biweek = GetBiweektime(physicianid, date);
            List<TimeSheetViewModel> timeSheets = timeSheetViewModels(biweek.Biweekid);
            List<ReimbursementViewModel> reimbursements = reimbursementViewModels(biweek.Biweekid);
            BiWeekViewModel model = new BiWeekViewModel();
            model.Id = biweek.Biweekid;
            model.Physicianid = (int)biweek.Physicianid;
            model.Firstday = (DateTime)biweek.Firstday;
            model.Lastday = (DateTime)biweek.Lastday;
            model.TimeSheets = timeSheets;
            model.Reimbursements = reimbursements;
            return model;
        }

        public Biweektime GetBiweektime(int physicianid, DateTime date)
        {
            var biweek = _db.Biweektimes.FirstOrDefault(x => x.Physicianid == physicianid && x.Firstday == date);
            if (biweek == null)
            {
                biweek = new Biweektime();
                biweek.Physicianid = physicianid;
                biweek.Isfinalized = false;
                biweek.Firstday = date;
                if (biweek.Firstday.Value.Day <= 14)
                {
                    biweek.Lastday = new DateTime(biweek.Firstday.Value.Year, biweek.Firstday.Value.Month, 14);
                }
                else
                {
                    biweek.Lastday = new DateTime(biweek.Firstday.Value.Year, biweek.Firstday.Value.Month, DateTime.DaysInMonth(biweek.Firstday.Value.Year, biweek.Firstday.Value.Month));
                }
                _db.Biweektimes.Add(biweek);
                _db.SaveChanges();
            }
            return biweek;
        }

        public List<TimeSheetViewModel> timeSheetViewModels(int biweekid)
        {
            var biweek = _db.Biweektimes.FirstOrDefault(x => x.Biweekid == biweekid);
            var physicianid = biweek.Physicianid;
            List<TimeSheetViewModel> timeSheets = new List<TimeSheetViewModel>();
            for (DateTime i = biweek.Firstday.Value; i <= biweek.Lastday; i = i.AddDays(1))
            {
                var thistimesheet = _db.Timesheets.FirstOrDefault(x => x.Physicianid == physicianid && x.Date == i);
                if (thistimesheet == null)
                {
                    thistimesheet = new Timesheet();
                    thistimesheet.Physicianid = physicianid;
                    thistimesheet.Isweekend = false;
                    TimeOnly shiftTime = _scheduling.TotalHrsofThisDateShifts((int)physicianid, i);
                    thistimesheet.Oncallhours = shiftTime.Hour + shiftTime.Minute / 60.0m;
                    if (_admin.GetSessionAdminId() != -1)
                    {
                        thistimesheet.Createdby = _admin.GetFirstOrDefault(x => x.Adminid == _admin.GetSessionAdminId()).Aspnetuserid;
                    }
                    if (_physician.GetSessionPhysicianId() != -1)
                    {
                        thistimesheet.Createdby = _physician.GetFirstOrDefault(x => x.Physicianid == physicianid).Aspnetuserid;
                    }
                    thistimesheet.Createddate = DateTime.Now;
                    thistimesheet.Housecall = 0;
                    thistimesheet.Consult = 0;
                    thistimesheet.Date = i;
                    thistimesheet.Biweektimeid = biweek.Biweekid;
                    _db.Timesheets.Add(thistimesheet);
                    _db.SaveChanges();
                }
                TimeSheetViewModel timeSheetViewModel = new TimeSheetViewModel();
                timeSheetViewModel.Id = (int)thistimesheet.Timesheetid;
                timeSheetViewModel.ThisDate = thistimesheet.Date.Value;
                timeSheetViewModel.IsWeekend = (bool)thistimesheet.Isweekend;
                timeSheetViewModel.OnCallHours = new TimeOnly((int)thistimesheet.Oncallhours.Value, (int)((thistimesheet.Oncallhours.Value - (int)thistimesheet.Oncallhours.Value) * 60)); ;
                timeSheetViewModel.HouseCalls = (int)thistimesheet.Housecall;
                timeSheetViewModel.PhoneCalls = (int)thistimesheet.Consult;
                timeSheets.Add(timeSheetViewModel);
            }
            return timeSheets;
        }

        public List<ReimbursementViewModel> reimbursementViewModels(int biweekid)
        {
            var biweek = _db.Biweektimes.FirstOrDefault(x => x.Biweekid == biweekid);
            var physicianid = biweek.Physicianid;
            List<ReimbursementViewModel> reimbursements = new List<ReimbursementViewModel>();
            for (DateTime i = biweek.Firstday.Value; i <= biweek.Lastday; i = i.AddDays(1))
            {
                var thisreimbursement = _db.Reimbursements.FirstOrDefault(x => x.Physicianid == physicianid && x.Date == i);
                if (thisreimbursement == null)
                {
                    thisreimbursement = new Reimbursement();
                    thisreimbursement.Physicianid = physicianid;
                    thisreimbursement.Amount = 0;
                    thisreimbursement.Isdeleted = false;
                    if (_admin.GetSessionAdminId() != -1)
                    {
                        thisreimbursement.Createdby = _admin.GetFirstOrDefault(x => x.Adminid == _admin.GetSessionAdminId()).Aspnetuserid;
                    }
                    if (_physician.GetSessionPhysicianId() != -1)
                    {
                        thisreimbursement.Createdby = _physician.GetFirstOrDefault(x => x.Physicianid == physicianid).Aspnetuserid;
                    }
                    thisreimbursement.Biweektimeid = biweek.Biweekid;
                    thisreimbursement.Date = i;
                    thisreimbursement.Createddate = DateTime.Now;
                    _db.Reimbursements.Add(thisreimbursement);
                    _db.SaveChanges();
                }
                ReimbursementViewModel reimbursementViewModel = new ReimbursementViewModel();
                reimbursementViewModel.Id = (int)thisreimbursement.Id;
                reimbursementViewModel.Item = thisreimbursement.Item;
                reimbursementViewModel.ThisDate = (DateTime)thisreimbursement.Date;
                reimbursementViewModel.Amount = (decimal)thisreimbursement.Amount;
                reimbursementViewModel.Billname = thisreimbursement.Bill;
                reimbursements.Add(reimbursementViewModel);
            }
            return reimbursements;
        }

        public Timesheet GetTimesheet(int physicianid, DateTime date)
        {
            return _db.Timesheets.FirstOrDefault(x => x.Physicianid == physicianid && x.Date == date);
        }

        public Reimbursement GetReimbursement(int physicianid, DateTime date)
        {
            return _db.Reimbursements.FirstOrDefault(x => x.Physicianid == physicianid && x.Date == date);
        }

        public void UpdateTimesheet(Timesheet timesheet)
        {
            _db.Timesheets.Update(timesheet);
            _db.SaveChanges();
        }

        public void UpdateReimbursement(Reimbursement reimbursement)
        {
            _db.Reimbursements.Update(reimbursement);
            _db.SaveChanges();
        }
    }
}
