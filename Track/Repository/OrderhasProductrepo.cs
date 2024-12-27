using Microsoft.EntityFrameworkCore;
using System.Linq;
using Track.Data;
using Track.Models;
using Track.Repository.Irepository;

namespace Track.Repository
{
    public class OrderhasProductrepo : MainRepo<OrderhasProducts>, Iorderhasproduct
    {
        private readonly Applicationdbcontext _db;
        private readonly DbSet<OrderhasProducts> _products;
        public OrderhasProductrepo(Applicationdbcontext db) : base(db)
        {
            _db = db;
            _products= db.Set<OrderhasProducts>();
        }

        public void Update(OrderhasProducts product)
        {
            OrderhasProducts To_Update= _products.FirstOrDefault(u=>u.Id==product.Id);
            if(To_Update!=null) 
            {
                To_Update.Order_id = product.Order_id;
                To_Update.Product_id= product.Product_id;
                To_Update.Quantity = product.Quantity;
                _products.Update(To_Update);
            }
        }
    }
}
