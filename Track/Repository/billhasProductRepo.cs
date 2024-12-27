using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Track.Data;
using Track.Models;
using Track.Repository.Irepository;

namespace Track.Repository
{
    public class billhasProductRepo : MainRepo<BillhasProductClass>, IbillhasProduct
    {
        private readonly DbSet<BillhasProductClass> _billhasProducts;
        private readonly Applicationdbcontext _db;
        public billhasProductRepo(Applicationdbcontext db) : base(db)
        {
            _billhasProducts = db.Set<BillhasProductClass>();
            _db = db;
        }

        public void Update(BillhasProductClass obj)
        {
            BillhasProductClass To_update= _billhasProducts.FirstOrDefault(u=>u.Id==obj.Id);
            if (To_update!=null) 
            {
                To_update.Bill_id = obj.Bill_id;
                To_update.product_id= obj.product_id;
                To_update.Quantity = obj.Quantity;
                To_update.Rate = obj.Rate;
                To_update.total= obj.total;
                To_update.Extra_items = obj.Extra_items;
                _billhasProducts.Update(To_update);
            }
        }

        //public override void DeleteMost(List<BillhasProductClass> list)
        //{
        //    foreach(var lili in list)
        //    {
        //        List<StockClass> myStock = _db.StockTable.Where(u=>u.billhasProduct_id==lili.Id).ToList();
        //        foreach(var stock in myStock)
        //        {
        //            stock.InStock = "Y";
        //            _db.StockTable.Update(stock);
        //        }
        //    }
        //    _db.billhasProduct.RemoveRange(list);            
        //    _db.SaveChanges();
        //}
    }
}
