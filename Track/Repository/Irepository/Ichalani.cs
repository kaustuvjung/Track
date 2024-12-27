using Track.Models;

namespace Track.Repository.Irepository
{
    public interface Ichalani: Imainrepo<ChalaniClass>
    {
        void Update(ChalaniClass chalani);
    }
}
