using System.Collections.Generic;
using ArmLancer.Data.Models;

namespace ArmLancer.Core.Interfaces
{
    public interface ICategoryService : ICrudService<Category>
    {
        IEnumerable<Category> GetAllRecursive(long? parentId = null);
        IEnumerable<Category> GetList(long? parentId);
    }
}