using System.Collections.Generic;
using ArmLancer.Data.Models;

namespace ArmLancer.Core.Interfaces
{
    public interface IJobSubmissionService : ICrudService<JobSubmission>
    {
        IEnumerable<JobSubmission> GetByJobId(long jobId);
        IEnumerable<JobSubmission> GetByClientId(long clinetId);
        JobSubmission GetByClientAndJobId(long clientId, long jobId);
        bool AlreadySubmitted(long clientId, long jobId);
        bool DoesClientHaveSubmission(long clientId, long submissionId);
    }
}