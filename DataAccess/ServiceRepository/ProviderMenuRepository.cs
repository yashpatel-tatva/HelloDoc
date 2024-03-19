﻿using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository.IServiceRepository;
using DataModels.AdminSideViewModels;
using HelloDoc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ServiceRepository
{
    public class ProviderMenuRepository : IProviderMenuRepository
    {
        private readonly IPhysicianRepository _physician;
        private readonly IAspNetUserRepository _userRepository;
        private readonly HelloDocDbContext _db;
        public ProviderMenuRepository(IPhysicianRepository physician , HelloDocDbContext helloDocDbContext , IAspNetUserRepository aspNetUserRepository) 
        {
            _physician = physician;
            _db = helloDocDbContext;
            _userRepository = aspNetUserRepository;
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
        public void ChangeNotification(List<int> checkedToUnchecked, List<int> uncheckedToChecked)
        {
            BitArray settrue = new BitArray(1);
            settrue[0] = true;
            BitArray setfalse = new BitArray(1);
            setfalse[0] = false;

            foreach(var physician in checkedToUnchecked)
            {
                var notification = _db.Physiciannotifications.FirstOrDefault(x => x.Pysicianid == physician);
                notification.Isnotificationstopped = setfalse;
                _db.Physiciannotifications.Update(notification);
            }
            foreach(var physician in uncheckedToChecked)
            {
                var notification = _db.Physiciannotifications.FirstOrDefault(x => x.Pysicianid == physician);
                notification.Isnotificationstopped = settrue;
                _db.Physiciannotifications.Update(notification);
            }
            _db.SaveChanges();
        }

        public PhysicianAccountViewModel GetPhysicianAccountById(int Physicianid)
        {
            var physician = _db.Physicians.Include(x => x.Aspnetuser).Include(r=>r.Physicianregions).FirstOrDefault(x => x.Physicianid == Physicianid);
            PhysicianAccountViewModel model = new PhysicianAccountViewModel();
            model.PhysicianId = Physicianid;
            model.Aspnetid = physician.Aspnetuserid;
            model.Username = physician.Aspnetuser.Username;
            model.Password = physician.Aspnetuser.Passwordhash;
            model.FirstName = physician.Firstname;
            model.LastName = physician.Lastname;
            model.Email = physician.Email;
            model.Phone = physician.Mobile;
            model.MedicalLicense = physician.Medicallicense;
            model.NPINumber = physician.Npinumber;
            model.SynchronizationEmail = physician.Syncemailaddress;
            List<int> regionid = new List<int>();
            foreach (var item in physician.Physicianregions)
            {
                regionid.Add(item.Regionid);
            }
            model.SelectedRegionCB = regionid;
            model.regions = _db.Regions.ToList();
            return model;
        }

        public void EditPersonalinfo(PhysicianAccountViewModel model)
        {
            var physician = _db.Physicians.Include(x => x.Aspnetuser).Include(r => r.Physicianregions).FirstOrDefault(x => x.Physicianid == model.PhysicianId);
            physician.Physicianid =  model.PhysicianId ;
            physician.Firstname =  model.FirstName ;
            physician.Lastname =  model.LastName ;
            physician.Email =  model.Email ;
            physician.Mobile =  model.Phone ;
            physician.Medicallicense =  model.MedicalLicense ;
            physician.Npinumber =  model.NPINumber;
            physician.Syncemailaddress = model.SynchronizationEmail;
            var RegionToDelete = physician.Physicianregions.Select(x => x.Regionid).Except(model.SelectedRegionCB);
            foreach (var item in RegionToDelete)
            {
                Physicianregion? regiontodelete = _db.Physicianregions
            .FirstOrDefault(ar => ar.Physicianid == model.PhysicianId && ar.Regionid == item);

                if (regiontodelete != null)
                {
                    _db.Physicianregions.Remove(regiontodelete);
                }
            }
            IEnumerable<int> regionsToAdd = model.SelectedRegionCB.Except(physician.Physicianregions.Select(x=>x.Regionid));

            foreach (var item in regionsToAdd)
            {
                Physicianregion newRegion = new Physicianregion
                {
                    Physicianid = model.PhysicianId,
                    Regionid = item,
                };
                _db.Physicianregions.Add(newRegion);
            }
            _db.SaveChanges();
            model.regions = _db.Regions.ToList();
            _db.Update(physician);
            _db.SaveChanges();
        }
    }
}
