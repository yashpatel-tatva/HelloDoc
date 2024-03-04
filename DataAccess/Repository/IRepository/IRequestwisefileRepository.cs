using HelloDoc.DataModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository
{
    public interface IRequestwisefileRepository : IRepository<Requestwisefile>
    {
        void Delete(int id);

        void Add(int id, List<IFormFile> formFiles);
    }
}
