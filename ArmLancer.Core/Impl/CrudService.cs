using System;
using System.Linq;
using ArmLancer.Core.Interfaces;
using ArmLancer.Data.Context;
using ArmLancer.Data.Models;

namespace ArmLancer.Core.Impl
{
    public class CrudService<T> : ICrudService<T> where T : AbstractEntityModel
    {
        protected readonly ArmLancerDbContext _context;
        protected readonly IServiceProvider _serviceProvider;
        
        public CrudService(IServiceProvider serviceProvider, ArmLancerDbContext context)
        {
            _serviceProvider = serviceProvider;
            _context = context;
        }
        
        public T Get(long id)
        {
            return _context.Find<T>(id);
        }

        public T Create(T entity)
        {
            var entry = _context.Add(entity);
            _context.SaveChanges();
            return entry.Entity;
        }

        public T Update(T entity)
        {
            var entry = _context.Attach(entity);
            _context.SaveChanges();
            return entry.Entity;
        }

        public void Delete(long id)
        {
            var dbEntity = _context.Find<T>(id);
            var trackingEntity = dbEntity as AbstractTrackingEntityModel;
            if (trackingEntity != null)
            {
                trackingEntity.Removed = DateTime.UtcNow;
            }
            else
            {
                _context.Remove(dbEntity);
            }
            _context.SaveChanges();
        }

        public bool Exists(long id)
        {
            var dbEntity = _context.Find<T>(id);
            return dbEntity != null;
        }

        public IQueryable<T> All => _context.Set<T>();
    }
}