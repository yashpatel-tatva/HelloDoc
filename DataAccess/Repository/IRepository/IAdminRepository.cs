using DataModels.AdminSideViewModels;
using HelloDoc;

namespace DataAccess.Repository.IRepository
{
    public interface IAdminRepository : IRepository<Admin>
    {
        void SetSession(Admin admin);
        void RemoveSession();
        int GetSessionAdminId();
        AdminProfileViewModel GetAdminProfile(int id);
        void Edit(int adminid, AdminProfileViewModel viewModel);
        void EditBillingDetails(int adminid, AdminProfileViewModel viewModel);
        void CreateAdmin(AdminProfileViewModel model);
    }
}
