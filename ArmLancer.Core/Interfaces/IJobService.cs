using System.Collections.Generic;
using ArmLancer.Data.Models;

namespace ArmLancer.Core.Interfaces
{
    public interface IJobService : ICrudService<Job>
    {
        bool IsInProgress(long jobId);
        void FinishJob(long jobId);
        bool DoesEmployeerOwnJob(long employeerId, long jobId);
        long? GetJobFreeLancerId(long jobId);
        IEnumerable<Job> GetByCategoryId(long categoryId);
    }
}