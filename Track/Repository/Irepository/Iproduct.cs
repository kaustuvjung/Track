namespace Track.Repository.Irepository
{
    public interface Iproduct : Imainrepo<Models.ProductClass>
    {
        void Update(Models.ProductClass product);
    }
}
