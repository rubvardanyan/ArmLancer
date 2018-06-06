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
            return _context.JobSubmissions.Any(js => js.JobId == jobId && js.Status == SubmissionStatus.Accepted);
        }

        public void AcceptSubmission(long submissionId)
        {
            var submission = this.Get(submissionId);
            var submissions = _context.JobSubmissions
                .Where(js => js.JobId == submission.JobId && js.Status == SubmissionStatus.Waiting).ToList();
            submissions.ForEach(s => s.Status = SubmissionStatus.Ignored);
            submission.Status = SubmissionStatus.Accepted;
            _context.SaveChanges();
        }
        
        public void DeclineSubmission(long submissionId)
        {
            var submission = this.Get(submissionId);
            submission.Status = SubmissionStatus.Declined;
            _context.SaveChanges();
        }
        
        public void CancelSubmission(long submissionId)
        {
            var submission = this.Get(submissionId);
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