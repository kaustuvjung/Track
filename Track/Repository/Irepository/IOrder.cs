using System.Collections.Generic;
using Track.Models;

namespace Track.Repository.Irepository
{
    public interface IOrder:Imainrepo<OrderClass>
    {
        void Update(OrderClass order);
        public IEnumerable<OrderClass> NewGetall();

    }
}
