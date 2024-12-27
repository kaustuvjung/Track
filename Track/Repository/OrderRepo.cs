using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Track.Data;
using Track.Models;
using Track.Repository.Irepository;

namespace Track.Repository
{
    public class OrderRepo : MainRepo<OrderClass>, IOrder
    {
        private readonly Applicationdbcontext _db;
        private DbSet<OrderClass> _dbSet;
        public OrderRepo(Applicationdbcontext db): base(db)
        {
            _db = db;
            _dbSet = db.Set<OrderClass>();
        }
        public void Update(OrderClass order)
        {
            OrderClass two = _dbSet.FirstOrDefault(u=>u.Id == order.Id);
            if (two != null)
            {
                two.Arival=order.Arival;
                 two.vendor=order.vendor;
                two.vendor_id=order.vendor_id;
                _dbSet.Update(two);
            }
        }

        public IEnumerable<OrderClass> NewGetall()
        {
            IQueryable<OrderClass> man= _dbSet.AsQueryable();
            //man = man.Include(u => u.Product);
            man = man.Include("vendor");
            return man.ToList();
        }
    }
}
