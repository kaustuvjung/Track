using Track.Models;

namespace Track.Repository.Irepository
{
    public interface ICustomer: Imainrepo<CustomerClass>
    {
        public void Update(CustomerClass customer);
    }
}
