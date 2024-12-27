namespace Track.Repository.Irepository
{
    public interface IunitOfwork
    {
        public IClient? Client { get;}
        public ICustomer? customer { get;}
        public IOrder? Order { get;}
        public Iproduct? Product { get;}
        public IStock? Stock { get;}
        public IVendor? Vendor { get;}

        public Iorderhasproduct? Orderhasproduct { get;}

        public IBucket? Bucket { get;}

        public Ibill? Bill { get;}

        public IbillhasProduct? Billhasproduct { get;}

        public IPayment? Payment { get;}

        public Ichalani Chalani { get;}

        public IChalanihasproduct Chalanihasproduct { get;}
        void Save();
    }
}
