using Track.Models;

namespace Track.Repository.Irepository
{
    public interface IStock: Imainrepo<StockClass>
    {
        void Update(StockClass stock);
    }
}
