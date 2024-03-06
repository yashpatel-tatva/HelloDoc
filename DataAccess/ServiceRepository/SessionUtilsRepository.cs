using DataAccess.ServiceRepository.IServiceRepository;
using DataModels.CommonViewModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ServiceRepository
{
    public class SessionUtilsRepository : ISessionUtilsRepository
    {
        public static LoggedInPersonViewModel GetLoggedInPerson(ISession session)
        {
            LoggedInPersonViewModel viewModel = null ;
            if(!string.IsNullOrEmpty(session.GetString("AspNetId")))
            {
                viewModel = new LoggedInPersonViewModel();
                viewModel.AspnetId = session.GetString("AspNetID");
                viewModel.UserName = session.GetString("UserName");
                viewModel.Role = session.GetString("Role");
            }
            return viewModel;
        }


        public static void SetLoggedInPerson(ISession session, LoggedInPersonViewModel viewModel)
        {
            if(viewModel != null)
            {
                session.SetString("AspNetId" , viewModel.AspnetId);
                session.SetString("UserName" , viewModel.UserName);
                session.SetString("Role" , viewModel.Role);
            }
        }
    }
}
