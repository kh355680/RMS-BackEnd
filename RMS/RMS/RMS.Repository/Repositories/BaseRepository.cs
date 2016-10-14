using System.Data.Entity;
using System.Linq;
using RMS.BusinessModel.Context;
using RMS.BusinessModel.Entities;
using RMS.Repository.IRepositories;

namespace RMS.Repository.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : Entity
    {
        protected RmsDbContext Context { get; set; }
        protected DbSet<TEntity> Table { get; set; } 

        public BaseRepository(RmsDbContext context)
        {
            Context = context;
            Table = Context.Set<TEntity>();
        } 

        public IQueryable<TEntity> Get()
        {
            return Table.AsQueryable();
        }

        public TEntity Get(string id)
        {
            return Table.Find(id);
        }

        public TEntity Insert(TEntity entity)
        {
            var entitySaved = Table.Add(entity);
            Save();

            return entitySaved;
        }

        public TEntity Update(TEntity entity)
        {
            var original = Get(entity.Id);

            if (original == null)
                return null;

            Context.Entry(original).CurrentValues.SetValues(entity);
            Save();

            return original;
        }

        public TEntity Remove(string id)
        {
            var item = Get(id);

            if (item == null)
                return null;

            var result = Table.Remove(item);
            Save();

            return result;
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }
}