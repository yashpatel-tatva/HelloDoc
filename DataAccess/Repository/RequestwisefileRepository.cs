using DataAccess.Repository.IRepository;
using HelloDoc;
using Microsoft.AspNetCore.Http;
using System.Collections;

namespace DataAccess.Repository
{
    public class RequestwisefileRepository : Repository<Requestwisefile>, IRequestwisefileRepository
    {
        private readonly IAdminRepository _admin;
        private readonly IPhysicianRepository _physician;

        public RequestwisefileRepository(HelloDocDbContext db, IAdminRepository adminRepository, IPhysicianRepository physician) : base(db)
        {
            _admin = adminRepository;
            _physician = physician;
        }

        public void Add(int id, List<IFormFile> formFiles)
        {
            foreach (var file in formFiles)
            {
                string filename = file.FileName;
                string filenameWithoutExtension = Path.GetFileNameWithoutExtension(filename);
                string extension = Path.GetExtension(filename);
                string filewith = filenameWithoutExtension + "_" + DateTime.Now.ToString("dd`MM`yyyy`HH`mm`ss") + extension;

                string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Documents", id.ToString());
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                string filePath = Path.Combine(directoryPath, filewith);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                Requestwisefile requestwisefile = new Requestwisefile();
                requestwisefile.Requestid = id;
                requestwisefile.Filename = filePath;
                requestwisefile.Createddate = DateTime.Now;
                requestwisefile.Adminid = _admin.GetSessionAdminId();
                if (_admin.GetSessionAdminId() != -1)
                {
                    requestwisefile.Adminid = _admin.GetSessionAdminId();
                }
                if (_physician.GetSessionPhysicianId() != -1)
                {
                    requestwisefile.Physicianid = _physician.GetSessionPhysicianId();
                }
                _db.Requestwisefiles.Add(requestwisefile);

            }
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var file = _db.Requestwisefiles.FirstOrDefault(x => x.Requestwisefileid == id);
            BitArray r = new BitArray(1);
            r[0] = true;
            file.Isdeleted = r;
            _db.Requestwisefiles.Update(file);
            _db.SaveChanges();
        }
    }
}
