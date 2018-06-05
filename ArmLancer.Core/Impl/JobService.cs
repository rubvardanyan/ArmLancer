using System;
using System.Collections.Generic;
using System.Linq;
using ArmLancer.Core.Interfaces;
using ArmLancer.Data.Context;
using ArmLancer.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ArmLancer.Core.Impl
{
    public class JobService : CrudService<Job>, IJobService
    {
        public JobService(IServiceProvider serviceProvider, ArmLancerDbContext context) : base(serviceProvider, context)
        {
        }

        public bool DoesEmployeerOwnJob(long employeerId, long jobId)
        {
            return _context.Jobs.SingleOrDefault(j => j.ClientId == employeerId && j.Id == jobId) != null;
        }

        public IEnumerable<Job> GetByCategoryId(long categoryId)
        {
            return _context.Jobs.Where(j => j.CategoryId == categoryId).AsNoTracking();
        }
    }
}