using DataModels.AdminSideViewModels;
using HelloDoc;

namespace DataAccess.Repository.IRepository
{
    public interface IOrderDetailRepository : IRepository<Orderdetail>
    {
        public void Add(SendOrderViewModel model);
    }
}
