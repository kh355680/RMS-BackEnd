using System.Linq;
using RMS.BusinessModel.Entities;

namespace RMS.Repository.IRepositories
{
    public interface IBaseRepository<TEntity> where TEntity : Entity
    {
        IQueryable<TEntity> Get();
        TEntity Get(string id);
        TEntity Insert(TEntity entity);
        TEntity Update(TEntity entity);
        TEntity Remove(string id);
        void Save();

    }
}