using Track.Models;

namespace Track.Repository.Irepository
{
    public interface IPayment: Imainrepo<PaymentClass>
    {
        void Update(PaymentClass payment);
    }
}
