using Microsoft.EntityFrameworkCore;
using System.Linq;
using Track.Data;
using Track.Models;
using Track.Repository.Irepository;

namespace Track.Repository
{
    public class CustomerRepo : MainRepo<CustomerClass>, ICustomer
    {
        private readonly Applicationdbcontext _db;
        private readonly DbSet<CustomerClass> _dbSet;
        public CustomerRepo(Applicationdbcontext db):base(db)
        {
            _db = db;
            _dbSet= db.Set<CustomerClass>();
        }
        public void Update(CustomerClass customer)
        {
            CustomerClass ToUpdate = new CustomerClass();
            ToUpdate=_dbSet.FirstOrDefault(u=>u.Id==customer.Id);
            if(ToUpdate!=null) 
            {
                ToUpdate.Name= customer.Name;
                ToUpdate.PhoneNumber=customer.PhoneNumber;
                ToUpdate.DistrictId= customer.DistrictId;
                ToUpdate.ProvinceId= customer.ProvinceId;
                ToUpdate.LocalBodyId= customer.LocalBodyId;
                ToUpdate.Address = customer.Address;
                _dbSet.Update(ToUpdate);
            }
        }
    }
}
