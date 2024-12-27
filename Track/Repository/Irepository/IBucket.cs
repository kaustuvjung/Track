using Track.Models;

namespace Track.Repository.Irepository
{
    public interface IBucket:Imainrepo<BucketClass>
    {
        void Update(BucketClass bucket);
    }
}
