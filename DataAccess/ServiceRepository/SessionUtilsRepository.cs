using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository.IServiceRepository;
using DataModels.CommonViewModel;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace DataAccess.ServiceRepository
{
    public class SessionUtilsRepository : ISessionUtilsRepository
    {
        private readonly IAdminRepository _admin;
        public SessionUtilsRepository(IAdminRepository adminRepository)
        {
            _admin = adminRepository;
        }
        public static LoggedInPersonViewModel GetLoggedInPerson(ISession session)
        {
            LoggedInPersonViewModel viewModel = null;
            if (!string.IsNullOrEmpty(session.GetString("AspNetId")))
            {
                viewModel = new LoggedInPersonViewModel();
                viewModel.AspnetId = session.GetString("AspNetID");
                viewModel.UserName = session.GetString("UserName");
                viewModel.Role = session.GetString("Role");
            }
            return viewModel;
        }

        public void SetAdminSeesion(string aspnetid)
        {
            _admin.SetSession(_admin.GetFirstOrDefault(x => x.Aspnetuserid == aspnetid));

        }

        public void SetLoggedInPerson(ISession session, LoggedInPersonViewModel viewModel)
        {
            if (viewModel != null)
            {
                session.SetString("AspNetId", viewModel.AspnetId);
                session.SetString("UserName", viewModel.UserName);
                session.SetString("Role", viewModel.Role);

            }
        }
    }
}
