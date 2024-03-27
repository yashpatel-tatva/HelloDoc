using DataAccess.Repository.IRepository;
using DataModels.AdminSideViewModels;
using HelloDoc;

namespace DataAccess.Repository
{
    public class OrderDetailRepository : Repository<Orderdetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(HelloDocDbContext db) : base(db)
        {
            _db = db;
        }

        public void Add(SendOrderViewModel model)
        {
            Orderdetail orderdetail = new Orderdetail();
            orderdetail.Vendorid = model.VendorId;
            orderdetail.Requestid = model.RequestId;
            orderdetail.Faxnumber = model.Fax;
            orderdetail.Email = model.Email;
            orderdetail.Businesscontact = model.Contact;
            orderdetail.Prescription = model.Prescription;
            orderdetail.Noofrefill = model.Refill;
            orderdetail.Createddate = DateTime.Now;
            orderdetail.Createdby = model.CreatedBy;
            _db.Orderdetails.Add(orderdetail);
            _db.SaveChanges();
        }
    }
}
