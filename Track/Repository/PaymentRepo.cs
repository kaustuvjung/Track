using Microsoft.EntityFrameworkCore;
using System.Linq;
using Track.Data;
using Track.Models;
using Track.Repository.Irepository;

namespace Track.Repository
{
    public class PaymentRepo : MainRepo<PaymentClass>, IPayment
    {
        private readonly DbSet<PaymentClass> _paymentClasses;
        public PaymentRepo(Applicationdbcontext db) : base(db)
        {
            _paymentClasses = db.Set<PaymentClass>();   
        }

        public void Update(PaymentClass payment)
        {
            PaymentClass obj = _paymentClasses.FirstOrDefault(u => u.Id == payment.Id);
            if (obj != null) 
            {
                obj.Bill_id = payment.Bill_id;
                obj.PDate = payment.PDate;
                obj.Method = payment.Method;
                obj.Amount = payment.Amount;
                _paymentClasses.Update(obj);
            }
        }
    }
}
