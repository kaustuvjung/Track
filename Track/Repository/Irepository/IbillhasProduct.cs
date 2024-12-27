using Track.Models;

namespace Track.Repository.Irepository
{
    public interface IbillhasProduct: Imainrepo<BillhasProductClass>
    {
        void Update(BillhasProductClass obj);
    }
}
