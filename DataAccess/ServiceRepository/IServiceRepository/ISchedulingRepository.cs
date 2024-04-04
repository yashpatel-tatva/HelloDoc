using DataModels.AdminSideViewModels;

namespace DataAccess.ServiceRepository.IServiceRepository
{
    public interface ISchedulingRepository
    {
        List<PhysicianData> GetPhysicianData();
        List<ShiftData> ShifsOfDate(DateTime datetoshow, int region, int status, int next);
        List<ShiftData> ShifsOfWeek(DateTime datetoshow, int region, int status, int next);
        List<ShiftData> ShifsOfMonth(DateTime datetoshow, int region, int status, int next);
    }
}
