using MerchantService.Repositories.Interface;
using MerchantService.Context;
using Microsoft.EntityFrameworkCore;

namespace MerchantService.Repositories
{
    public class GeneralRepository<Context, Entity, Key> : IRepository<Entity, Key>
        where Entity : class
        where Context : DbContext
    {
        private readonly DbContext context;
        private readonly DbSet<Entity> DbSet;

        public GeneralRepository(DbContext context)
        {
            this.context = context;
            DbSet = context.Set<Entity>();
        }

        public async Task<int> Delete(Key key)
        {
            var entity = await GetById(key);
            if (entity is null)
            {
                return 0;
            }
            context.Set<Entity>().Remove(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Entity>> GetAll()
        {
            return await context.Set<Entity>().ToListAsync();
        }

        public async Task<Entity?> GetById(Key key)
        {
            if (key == null)
            {
                return null;
            }
            return await context.Set<Entity>().FindAsync(key);
        }

        public async Task<int> Insert(Entity entity)
        {
            await DbSet.AddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> Update(Entity entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            return await context.SaveChangesAsync();
        }
    }
}
