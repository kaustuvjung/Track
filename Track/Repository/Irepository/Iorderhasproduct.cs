using Track.Models;

namespace Track.Repository.Irepository
{
    public interface Iorderhasproduct: Imainrepo<OrderhasProducts>
    {
        public void Update(OrderhasProducts product);
    }
}
