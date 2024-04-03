using DataModels.AdminSideViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ServiceRepository.IServiceRepository
{
    public interface ISchedulingRepository
    {
        List<PhysicianData> GetPhysicianData();
        List<ShiftData> ShifsOfDate(DateTime datetoshow, int region, int status , int next);
        List<ShiftData> ShifsOfWeek(DateTime datetoshow, int region, int status , int next);
        List<ShiftData> ShifsOfMonth(DateTime datetoshow, int region, int status , int next);
    }
}
