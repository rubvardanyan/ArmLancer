using System.Collections.Generic;
using ArmLancer.Data.Models;

namespace ArmLancer.Core.Interfaces
{
    public interface IJobService : ICrudService<Job>
    {
        IEnumerable<Job> GetByCategoryId(long categoryId);
    }
}