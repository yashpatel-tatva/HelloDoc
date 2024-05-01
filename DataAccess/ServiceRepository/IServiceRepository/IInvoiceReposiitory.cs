using DataModels.AdminSideViewModels;
using HelloDoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ServiceRepository.IServiceRepository
{
    public interface IInvoiceReposiitory
    {
        Biweektime GetBiweektime(int physicianid, DateTime date);
        List<TimeSheetViewModel> timeSheetViewModels(int biweekid);
        List<ReimbursementViewModel> reimbursementViewModels(int biweekid);

        Timesheet GetTimesheet(int physicianid , DateTime date);
        Reimbursement GetReimbursement(int physicianid, DateTime date);

        BiWeekViewModel BiWeekData(int physicianid, DateTime date);
        void Editnightshiftpayrate(int physicianid, int rate);
        void Editshiftpayrate(int physicianid, int rate);
        void Editnighthousecallpayrate(int physicianid, int rate);
        void Editconsultpayrate(int physicianid, int rate);
        void Editnightconsultpayrate(int physicianid, int rate);
        void Editbatchtestingpayrate(int physicianid, int rate);
        void Edithousecallpayrate(int physicianid, int rate);

        void UpdateTimesheet(Timesheet timesheet);
        void UpdateReimbursement(Reimbursement reimbursement);

    }
}
