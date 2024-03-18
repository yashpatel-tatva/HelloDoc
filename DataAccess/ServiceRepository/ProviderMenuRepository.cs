using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository.IServiceRepository;
using DataModels.AdminSideViewModels;
using HelloDoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ServiceRepository
{
    public class ProviderMenuRepository : IProviderMenuRepository
    {
        private readonly IPhysicianRepository _physician;
        private readonly HelloDocDbContext _db;
        public ProviderMenuRepository(IPhysicianRepository physician , HelloDocDbContext helloDocDbContext) 
        {
            _physician = physician;
            _db = helloDocDbContext;
        }
        public List<ProviderMenuViewModel> GetAllProviderDetailToDisplay(int region, int order)
        {

            var physicians = _physician.getAll();
            if(region != 0)
            {
                physicians = physicians.Where(x=>x.Regionid == region).ToList();
            }
            if(order == 1)
            {
                physicians = physicians.OrderByDescending(x=>x.Firstname).ToList();
            }
            else
            {
                physicians = physicians.OrderBy(x => x.Firstname).ToList();
            }
            List<ProviderMenuViewModel> providers = new List<ProviderMenuViewModel>(); 
            foreach(var physician in physicians)
            {
                ProviderMenuViewModel model = new ProviderMenuViewModel();
                model.PhysicanId = physician.Physicianid;
                model.Name = physician.Firstname + " " + physician.Lastname;
                model.StopNotification = physician.Physiciannotifications.ElementAt(0).Isnotificationstopped.Get(0);
                model.Role = _db.Aspnetroles.FirstOrDefault(x => x.Id == (_db.Aspnetuserroles.FirstOrDefault(x=>x.Userid == physician.Aspnetuserid).Roleid)).Name;
                model.OnCallStatus = "Active";
                if (physician.Status == 0)
                    model.Status = "Not Active";
                else if (physician.Status == 1)
                    model.Status = "Active";
                else
                    model.Status = "Pending";
                model.Email = physician.Email;
                providers.Add(model);
            }
            return providers;
        }
    }
}
