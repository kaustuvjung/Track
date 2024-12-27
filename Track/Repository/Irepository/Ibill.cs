using Track.Models;

namespace Track.Repository.Irepository
{
    public interface Ibill: Imainrepo<BillClass>
    {
        public void Update(BillClass bill);
    }
}
