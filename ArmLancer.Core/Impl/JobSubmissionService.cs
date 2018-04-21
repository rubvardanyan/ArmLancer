using System;
using System.Collections.Generic;
using System.Linq;
using ArmLancer.Core.Interfaces;
using ArmLancer.Data.Context;
using ArmLancer.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ArmLancer.Core.Impl
{
    public class JobSubmissionService : CrudService<JobSubmission>, IJobSubmissionService
    {
        public JobSubmissionService(IServiceProvider serviceProvider, ArmLancerDbContext context) : base(serviceProvider, context)
        {
        }

        public IEnumerable<JobSubmission> GetByJobId(long jobId)
        {
            return _context.JobSubmissions.Where(js => js.JobId == jobId).AsNoTracking();
        }

        public IEnumerable<JobSubmission> GetByClientId(long clinetId)
        {
            return _context.JobSubmissions.Where(js => js.ClientId == clinetId).AsNoTracking();
        }
    }
}