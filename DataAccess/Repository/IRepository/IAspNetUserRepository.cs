using HelloDoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository
{
    public interface IAspNetUserRepository : IRepository<Aspnetuser>
    {

        bool checkpass(Aspnetuser user);
        bool checkemail(Aspnetuser user);


        void Update(Aspnetuser user);   
        void Save();
    }
}
