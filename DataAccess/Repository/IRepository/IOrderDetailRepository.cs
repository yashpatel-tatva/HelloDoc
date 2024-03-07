using DataModels.AdminSideViewModels;
using HelloDoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository
{
    public interface IOrderDetailRepository : IRepository<Orderdetail>
    {
        public void Add(SendOrderViewModel model);
    }
}
