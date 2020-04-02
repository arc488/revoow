using Revoow.Data.IRepositories;
using Revoow.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Revoow.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {

        protected readonly AppDbContext appDbContext;
        protected readonly DbSet<TEntity> entities;

        public Repository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
            this.entities = this.appDbContext.Set<TEntity>();
        }

        public virtual void Add(TEntity entity)
        {
            this.entities.Add(entity);
            this.appDbContext.SaveChanges();
        }

        public virtual void Delete(TEntity entity)
        {
            this.entities.Remove(entity);
            this.appDbContext.SaveChanges();
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return this.entities;
        }

        public virtual TEntity GetById(int id)
        {
            return this.entities.Find(id);
        }

        public virtual void Update(TEntity entity)
        {
            this.entities.Update(entity);
            this.appDbContext.SaveChanges();
        }
    }
}
