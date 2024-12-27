using Microsoft.EntityFrameworkCore;
using System.Linq;
using Track.Data;
using Track.Models;
using Track.Repository.Irepository;

namespace Track.Repository
{
    public class StockRepo : MainRepo<StockClass>, IStock
    {
        private readonly Applicationdbcontext _db;
        private readonly DbSet<StockClass> _stock;
        public StockRepo(Applicationdbcontext db):base(db)
        {
            _db = db;
            _stock= db.Set<StockClass>();
        }
        public void Update(StockClass stock)
        {
            StockClass one = _stock.FirstOrDefault(a=>a.Id==stock.Id);
            if (one!=null) 
            {
                one.Customer_id=stock.Customer_id;
                one.billhasProduct_id=stock.billhasProduct_id;
                one.Product_id=stock.Product_id;
                one.billhasProduct_id=stock.billhasProduct_id;
                one.isDamaged=stock.isDamaged;
                one.InStock=stock.InStock;
                _stock.Update(one);
            }
        }
    }
}
