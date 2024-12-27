using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Linq;
using Track.Data;
using Track.Models;
using Track.Repository.Irepository;

namespace Track.Repository
{
    public class ProductRepo : MainRepo<ProductClass>, Iproduct
    {
        private readonly Applicationdbcontext _db;
        private readonly DbSet<ProductClass> _products;
        public ProductRepo(Applicationdbcontext db): base(db)
        {
            _db = db;
            _products=db.Set<ProductClass>();
        }
        public void Update(ProductClass product)
        {
            ProductClass one = _products.FirstOrDefault(u=>u.Id == product.Id);
            if (one != null) 
            {
                one.company = product.company;
                one.Name = product.Name;
                one.Description = product.Description;
                one.Category = product.Category;
                _products.Update(one);

            }

        }
    }
}
