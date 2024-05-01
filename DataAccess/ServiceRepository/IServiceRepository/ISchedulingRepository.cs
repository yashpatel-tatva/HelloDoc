using DataModels.AdminSideViewModels;

namespace DataAccess.ServiceRepository.IServiceRepository
{
    public interface ISchedulingRepository
    {
        List<PhysicianData> GetPhysicianData();
        List<ShiftData> ShifsOfDate(DateTime datetoshow, int region, int status, int next);
        List<ShiftData> ShifsOfWeek(DateTime datetoshow, int region, int status, int next);
        List<ShiftData> ShifsOfMonth(DateTime datetoshow, int region, int status, int next);
        List<ShiftData> ShifsOfDateforMonth(DateTime datetoshow, int region, int status, int next, int pagesize);
        List<ShiftData> ShifsOfDateOfProvider(DateTime datetoshow, int status, int next, int physicianid, int pagesize);
        List<ShiftData> ShifsOfMonth(DateTime datetoshow, int region, int v1, int currentpage, string v2);
        List<ShiftData> ShifsOfDate(DateTime datetoshow, int region, int v1, int currentpage, string v2);
        List<ShiftData> ShifsOfWeek(DateTime datetoshow, int region, int v1, int currentpage, string v2);        
        TimeOnly TotalHrsofThisDateShifts(int physicianid, DateTime date);

    }
}
