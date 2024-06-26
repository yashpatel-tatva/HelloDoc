﻿using HelloDoc;

namespace DataModels.AdminSideViewModels
{
    public class TimeSheetViewModel
    {
        public int Id { get; set; }
        public DateTime ThisDate { get; set; }
        public bool IsWeekend { get; set; }
        public TimeOnly OnCallHours { get; set; }
        public int HouseCalls { get; set; }
        public int PhoneCalls { get; set; }
    }

    public class ReimbursementViewModel
    {
        public int Id { get; set; }
        public string Item { get; set; }
        public DateTime ThisDate { get; set; }
        public decimal Amount { get; set; }
        public string Bill { get; set; }
        public string Billname { get; set; }
    }

    public class AdminReimbursementView
    {
        public int BiWeekId { get; set; }
        public bool IsAdmin { get; set; }
        public List<ReimbursementViewModel> Reimbursements { get; set; }
        public decimal bonus { get; set; }
        public string Description { get; set; }
    }

    public class BiWeekViewModel
    {
        public int Id { get; set; }
        public int Physicianid { get; set; }
        public DateTime Firstday { get; set; }
        public DateTime Lastday { get; set; }
        public bool isfinalized { get; set; }
        public bool isapproved { get; set; }

        public List<TimeSheetViewModel> TimeSheets { get; set; }
        public List<ReimbursementViewModel> Reimbursements { get; set; }

        public bool IsAdmin { get; set; }
        public Payrate Payrate { get; set; }
        public decimal shiftpay { get; set; }
        public decimal nightshiftpay { get; set; }
        public decimal housecallpay { get; set; }
        public decimal consultpay { get; set; }
        public decimal totalpay { get; set; }
    }

    public class payrates
    {
        public decimal shiftpay { get; set; }
        public decimal nightshiftpay { get; set; }
        public decimal housecallpay { get; set; }
        public decimal consultpay { get; set; }
        public decimal totalpay { get; set; }
    }

    public class TimeSheetOnlyView
    {
        public DateTime ShiftDate { get; set; }
        public int Shift { get; set; }
        public int NightShift { get; set; }
        public int HouseCall { get; set; }
        public int PhoneCall { get; set; }
        public int HouseCallNight { get; set; }
        public int PhoneCallNight { get; set; }
        public int BatchTesting { get; set; }

    }

    public class ReimbursementOnlyView
    {
        public int Id { get; set; }
        public DateTime ItemDate { get; set; }
        public string ItemName { get; set; }
        public decimal ItemAmount { get; set; }
        public string BillName { get; set; }
        public string BillPath { get; set; }
    }

    public class BiWeekOnlyView
    {
        public int Id { get; set; }
        public int PhysicianId { get; set; }
        public DateTime Firstday { get; set; }
        public List<TimeSheetOnlyView> TimeSheets { get; set; }
        public List<ReimbursementOnlyView> Reimbursements { get; set; }



    }
}
