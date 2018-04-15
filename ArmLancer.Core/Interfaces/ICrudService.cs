using System.Linq;
using ArmLancer.Data.Models;

namespace ArmLancer.Core.Interfaces
{
    public interface ICrudService<T> where T : AbstractEntityModel
    {
        T Get(long id);

        T Create(T entity);

        T Update(T entity);

        void Delete(long id);
        
        IQueryable<T> All { get; }
    }
}