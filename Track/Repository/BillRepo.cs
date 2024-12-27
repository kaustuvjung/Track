using Microsoft.EntityFrameworkCore;
using System.Linq;
using Track.Data;
using Track.Models;
using Track.Repository.Irepository;

namespace Track.Repository
{
    public class BillRepo : MainRepo<BillClass>, Ibill
    {
        private readonly DbSet<BillClass> _billSet;
        public BillRepo(Applicationdbcontext db) : base(db)
        {
            _billSet = db.Set<BillClass>();
        }

        public void Update(BillClass bill)
        {
            BillClass billClass= _billSet.FirstOrDefault(u=>u.Id==bill.Id);
            if(billClass != null ) 
            {
                billClass.Billno = bill.Billno;
                billClass.Remark= bill.Remark;
                billClass.total = bill.total;
                billClass.Date = bill.Date;
                billClass.Customer_id = bill.Customer_id;
                _billSet.Update(billClass);
            }
        }
    }
}
