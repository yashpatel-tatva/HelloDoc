using DataModels.AdminSideViewModels;
using HelloDoc;
using HelloDoc.Areas.PatientArea.ViewModels;

namespace DataAccess.ServiceRepository.IServiceRepository
{
    public interface IAllRequestDataRepository
    {
        List<AllRequestDataViewModel> FilteredRequest(List<Request> request);

        RequestDataViewModel GetRequestById(int id);
        RequestNotesViewModel GetNotesById(int id);
        void SaveAdminNotes(int id, RequestNotesViewModel model);

        byte[] DownloadExcle(List<AllRequestDataViewModel> model);
        void EditEmailPhone(RequestDataViewModel model);
        RequestViewUploadsViewModel GetDocumentByRequestId(int id);
        void AddRequestasAdmin(FamilyRequestViewModel model);
        void SaveProviderNote(int requestid, string note);
    }
}
