using HelloDoc;

namespace DataAccess.Repository.IRepository
{
    public interface IShiftDetailRepository : IRepository<Shiftdetail>
    {
        List<Shiftdetail> getall();
        void DeleteThisShift(int shiftdetailid, string modifiedby);
        bool EditShiftDetail(int shiftdetailid, DateOnly currentDate, DateTime startTimewithdate, DateTime endTimewithdate, string modifiedby);
        void ReturnThisShift(int shiftdetailid, string modifiedby);
    }
}
