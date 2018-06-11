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
    public class JobSubmissionService : CrudService<JobSubmission>, IJobSubmissionService
    {
        public JobSubmissionService(IServiceProvider serviceProvider, ArmLancerDbContext context) : base(serviceProvider, context)
        {
        }

        public JobSubmission GetByClientAndJobId(long clientId, long jobId)
        {
            return _context.JobSubmissions.SingleOrDefault(js => js.ClientId == clientId && js.JobId == jobId);
        }

        public bool AlreadySubmitted(long clientId, long jobId)
        {
            return _context.JobSubmissions.Any(js => js.JobId == jobId && js.ClientId == clientId);
        }

        public bool DoesClientHaveSubmission(long clientId, long submissionId)
        {
            return _context.JobSubmissions.Any(js => js.Id == submissionId && js.ClientId == clientId);
        }

        public bool AlreadyAcceptedOtherSubmit(long jobId)
        {
            return _context.Jobs.Find(jobId).Status != JobStatus.Waiting;
        }

        public void AcceptSubmission(long submissionId)
        {
            var submission = Get(submissionId);
            var job = _context.Jobs.Find(submission.JobId);
            job.Status = JobStatus.InProgress;
            submission.Status = SubmissionStatus.Accepted;
            _context.SaveChanges();
        }
        
        public void DeclineSubmission(long submissionId)
        {
            var submission = Get(submissionId);
            submission.Status = SubmissionStatus.Declined;
            _context.SaveChanges();
        }
        
        public void CancelSubmission(long submissionId)
        {
            var submission = Get(submissionId);
            submission.Status = SubmissionStatus.Cancelled;
            _context.SaveChanges();
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