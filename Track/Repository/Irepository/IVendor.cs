using Track.Models;

namespace Track.Repository.Irepository
{
    public interface IVendor:Imainrepo<VendorClass>
    {
        void Update(VendorClass obj);
    }
}
