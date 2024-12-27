using Microsoft.EntityFrameworkCore;
using System.Linq;
using Track.Data;
using Track.Models;
using Track.Repository.Irepository;

namespace Track.Repository
{
    public class ChalanihasProductRepo : MainRepo<ChalanihasProductClass>, IChalanihasproduct
    {
        private readonly DbSet<ChalanihasProductClass> _dbSet;
        public ChalanihasProductRepo(Applicationdbcontext db) : base(db)
        {
            _dbSet= db.Set<ChalanihasProductClass>();
        }

        public void Update(ChalanihasProductClass product)
        {
            ChalanihasProductClass obj= _dbSet.FirstOrDefault(t=>t.Id==product.Id);
            if (obj!=null) 
            {
                obj.product_id = product.product_id;
                obj.User_id=product.User_id;
                obj.Chalani_id = product.Chalani_id;
                obj.Quantity = product.Quantity;
                _dbSet.Update(obj);
            }
        }
    }
}
