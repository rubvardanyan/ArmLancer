using System.Collections.Generic;
using ArmLancer.Data.Models;

namespace ArmLancer.Core.Interfaces
{
    public interface IJobService : ICrudService<Job>
    {
        bool DoesEmployeerOwnJob(long employeerId, long jobId);
        IEnumerable<Job> GetByCategoryId(long categoryId);
    }
}