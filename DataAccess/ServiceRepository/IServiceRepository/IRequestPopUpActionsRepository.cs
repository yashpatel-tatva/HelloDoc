using DataModels.AdminSideViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ServiceRepository.IServiceRepository
{
    public interface IRequestPopUpActionsRepository
    {
        void AssignCase(int requestId, int physicianId, int assignby , string description);
        void BlockCase(DashpopupsViewModel model);

        void CancelCase(DashpopupsViewModel model);
    }
}
