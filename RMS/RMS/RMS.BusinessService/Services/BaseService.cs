using System;
using System.Linq;
using RMS.BusinessModel.Entities;
using RMS.BusinessService.IServices;
using RMS.Repository.Repositories;

namespace RMS.BusinessService.Services
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : Entity
    {
        public BaseRepository<TEntity> Repository { get; set; }

        public BaseService(BaseRepository<TEntity> repository)
        {
            Repository = repository;
        } 
         
        public IQueryable<TEntity> Get()
        {
           return Repository.Get().AsQueryable();
        }

        public TEntity Get(string id)
        {
            return Repository.Get(id);
        }

        public virtual TEntity Insert(TEntity entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            var result = Repository.Insert(entity);
            return result;
        }

        public TEntity Update(TEntity entity)
        {
            return Repository.Update(entity);
        }

        public TEntity Remove(string id)
        {
            return Repository.Remove(id);
        }
    }
}