using System.Linq;
using RMS.BusinessModel.Entities;

namespace RMS.BusinessService.IServices
{
    public interface IBaseService<TEntity> where TEntity : Entity
    {
        IQueryable<TEntity> Get();
        TEntity Get(string id);
        TEntity Insert(TEntity entity);
        TEntity Update(TEntity entity);
        TEntity Remove(string id);
    }
}