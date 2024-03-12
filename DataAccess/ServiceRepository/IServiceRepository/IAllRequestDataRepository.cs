﻿using DataAccess.Repository;
using DataAccess.Repository.IRepository;
using DataModels.AdminSideViewModels;
using HelloDoc.Areas.PatientArea.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ServiceRepository.IServiceRepository
{
    public interface IAllRequestDataRepository 
    {
        List<AllRequestDataViewModel> Status(int status);

        RequestDataViewModel GetRequestById(int id);
        RequestNotesViewModel GetNotesById(int id);
        void SaveAdminNotes(int id, RequestNotesViewModel model);

        byte[] DownloadExcle(string status);
        void EditEmailPhone(RequestDataViewModel model);
        RequestViewUploadsViewModel GetDocumentByRequestId(int id);
        void AddRequestasAdmin(FamilyRequestViewModel model);
    }
}
