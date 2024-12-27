using Track.Models;

namespace Track.Repository.Irepository
{
    public interface IClient: Imainrepo<ClinetClass>
    {
        void Update(ClinetClass clinet);
    }
}
