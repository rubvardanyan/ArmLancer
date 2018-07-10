using System;
using System.Collections.Generic;
using System.Linq;
using ArmLancer.Core.Interfaces;
using ArmLancer.Data.Context;
using ArmLancer.Data.Models;
using ArmLancer.Data.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace ArmLancer.Core.Impl
{
    public class RatingService : CrudService<Rating>, IRatingService
    {
        public RatingService(IServiceProvider serviceProvider, ArmLancerDbContext context) : base(serviceProvider,
            context)
        {
        }

        public IEnumerable<Rating> GetByClientTo(long clientId)
        {
            return _context.Ratings.Where(r => r.ClientIdTo == clientId).AsNoTracking();
        }
        
        public bool FreeLancerCanWriteReview(long jobId, long clientIdFrom)
        {
            return _context.Jobs.Where(j => j.Id == jobId && j.Status == JobStatus.Finished).Join(_context.JobSubmissions, j => j.Id, js => js.JobId,
                (j, js) => new {Job = j, JobSubmission = js}).Any(jjs => jjs.JobSubmission.ClientId == clientIdFrom && jjs.JobSubmission.Status == SubmissionStatus.Accepted);
        }

        public bool EmployeerCanWriteReview(long jobId, long clientIdFrom)
        {
            return true;
        }
    }
}