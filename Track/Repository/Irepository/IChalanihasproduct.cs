using Track.Models;

namespace Track.Repository.Irepository
{
    public interface IChalanihasproduct: Imainrepo<ChalanihasProductClass>
    {
        void Update(ChalanihasProductClass product);
    }
}
