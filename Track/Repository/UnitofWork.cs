using Track.Data;
using Track.Repository.Irepository;

namespace Track.Repository
{
    public class UnitofWork : IunitOfwork
    {
        private readonly Applicationdbcontext _db;
        public IClient? Client { get; private set; }

        public IOrder? Order { get; private set; }

        public Iproduct? Product { get; private set; }
       
        public IStock? Stock { get; private set; }

        public IVendor? Vendor { get; private set; }

        public ICustomer? customer { get; private set;}

        public Iorderhasproduct? Orderhasproduct { get; private set; }

        public IBucket? Bucket { get; private set; }

        public Ibill? Bill {  get; private set; }

        public IbillhasProduct? Billhasproduct { get; private set; }

        public IPayment? Payment { get; private set; }

        public Ichalani Chalani { get; private set; }

        public IChalanihasproduct Chalanihasproduct { get; private set; }

        public UnitofWork(Applicationdbcontext db)
        {
            _db = db;
            Client= new ClientRepo(db);
            Order= new OrderRepo(db);
            Product= new ProductRepo(db);
           Stock= new StockRepo(db);
            Vendor= new VendorRepo(db);
            customer= new CustomerRepo(db);
            Orderhasproduct = new OrderhasProductrepo(db);
            Bucket= new BucketRepo(db);
            Bill= new BillRepo(db);
            Billhasproduct= new billhasProductRepo(db);
            Payment= new PaymentRepo(db);
            Chalani = new ChalaniRepo(db);
            Chalanihasproduct = new ChalanihasProductRepo(db);

        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
