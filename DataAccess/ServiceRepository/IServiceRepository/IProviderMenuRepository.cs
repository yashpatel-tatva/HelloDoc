using DataModels.AdminSideViewModels;
using Microsoft.AspNetCore.Http;

namespace DataAccess.ServiceRepository.IServiceRepository
{
    public interface IProviderMenuRepository
    {
        List<ProviderMenuViewModel> GetAllProviderDetailToDisplay(int region, int order);
        void ChangeNotification(List<int> checkedToUnchecked, List<int> uncheckedToChecked);
        PhysicianAccountViewModel GetPhysicianAccountById(int Physicianid);
        void EditPersonalinfo(PhysicianAccountViewModel viewModel);
        void EditProviderMailingInfo(PhysicianAccountViewModel viewModel);
        void EditProviderAuthenticationInfo(PhysicianAccountViewModel model);
        void EditProviderAdminNote(int physicianid, string adminnote);
        void EditProviderPhoto(int physicianid, string base64string);
        void EditProviderSign(int physicianid, string base64string);
        void AddICA(int physicianid, IFormFile file);
        void AddBackDoc(int physicianid, IFormFile file);
        void AddTrainingDoc(int physicianid, IFormFile file);
        void AddNDA(int physicianid, IFormFile file);
        void AddLicense(int physicianid, IFormFile file);
        void AddCredential(int physicianid, IFormFile file);
        void DeleteThisAccount(int physicianid);
        int AddAccount(PhysicianAccountViewModel physicianAccountViewModel);
    }
}
