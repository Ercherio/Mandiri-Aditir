namespace MerchantService.Repositories.Interface
{
    public interface IRepository<Entity, Key> where Entity : class
    {
        Task<IEnumerable<Entity>> GetAll();
        Task<Entity?> GetById(Key key);
        Task<int> Insert(Entity entity);
        Task<int> Update(Entity entity);
        Task<int> Delete(Key key);

    }
}
