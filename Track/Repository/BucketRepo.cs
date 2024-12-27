using Microsoft.EntityFrameworkCore;
using System.Linq;
using Track.Data;
using Track.Models;
using Track.Repository.Irepository;

namespace Track.Repository
{
    public class BucketRepo : MainRepo<BucketClass>, IBucket
    {
        
        private readonly DbSet<BucketClass> _buckets;
        public BucketRepo(Applicationdbcontext db) : base(db)
        {
            _buckets= db.Set<BucketClass>();
        }

        public void Update(BucketClass bucket)
        {
            BucketClass man = _buckets.FirstOrDefault(u=>u.Id== bucket.Id);
            if(man!=null) 
            { 
                man.Order_id = bucket.Order_id;
                man.Product_id = bucket.Product_id;
            }
        }
    }
}
