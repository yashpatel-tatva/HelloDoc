using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ServiceRepository.IServiceRepository
{
    public interface IDocumentsRepository
    {
        void DeleteFile(int id);
        public byte[] Download(int id);
    }
}
