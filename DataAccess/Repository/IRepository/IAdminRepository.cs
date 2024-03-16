using DataModels.AdminSideViewModels;
using HelloDoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository
{
    public interface IAdminRepository : IRepository<Admin>
    {
        void SetSession(Admin admin);
        void RemoveSession();
        int GetSessionAdminId();
        AdminProfileViewModel GetAdminProfile(int id);
        void Edit(int adminid ,AdminProfileViewModel viewModel);
        void EditBillingDetails(int adminid ,AdminProfileViewModel viewModel);
    }
}
