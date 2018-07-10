using System;
using System.Collections.Generic;
using System.Linq;
using ArmLancer.Core.Interfaces;
using ArmLancer.Data.Context;
using ArmLancer.Data.Models;
using ArmLancer.Data.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace ArmLancer.Core.Impl
{
    public class JobService : CrudService<Job>, IJobService
    {
        public JobService(IServiceProvider serviceProvider, ArmLancerDbContext context) : base(serviceProvider, context)
        {
        }

        public bool IsInProgress(long jobId)
        {
            return Get(jobId).Status == JobStatus.InProgress;
        }

        public void FinishJob(long jobId)
        {
            var job = Get(jobId);
            job.Status = JobStatus.Finished;
            _context.SaveChanges();
        }

        public bool DoesEmployeerOwnJob(long employeerId, long jobId)
        {
            return _context.Jobs.Any(j => j.Id == jobId && j.ClientId == employeerId);
        }

        public long? GetJobFreeLancerId(long jobId)
        {
            return _context.JobSubmissions.SingleOrDefault(js =>
                js.JobId == jobId && js.Status == SubmissionStatus.Accepted)?.ClientId;
        }

        public IEnumerable<Job> GetByCategoryId(long categoryId)
        {
            return _context.Jobs.Where(j => j.CategoryId == categoryId).AsNoTracking();
        }
    }
}