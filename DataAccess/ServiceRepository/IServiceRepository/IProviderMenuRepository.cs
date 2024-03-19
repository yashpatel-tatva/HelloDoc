using DataModels.AdminSideViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ServiceRepository.IServiceRepository
{
    public interface IProviderMenuRepository
    {
        List<ProviderMenuViewModel> GetAllProviderDetailToDisplay(int region, int order);
        void ChangeNotification(List<int> checkedToUnchecked, List<int> uncheckedToChecked);
        PhysicianAccountViewModel GetPhysicianAccountById(int Physicianid);
        void EditPersonalinfo(PhysicianAccountViewModel viewModel);
    }
}
