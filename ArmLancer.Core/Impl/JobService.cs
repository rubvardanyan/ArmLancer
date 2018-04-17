using System;
using System.Collections.Generic;
using System.Linq;
using ArmLancer.Core.Interfaces;
using ArmLancer.Data.Context;
using ArmLancer.Data.Models;

namespace ArmLancer.Core.Impl
{
    public class JobService : CrudService<Job>, IJobService
    {
        public JobService(IServiceProvider serviceProvider, ArmLancerDbContext context) : base(serviceProvider, context)
        {
        }

        public IEnumerable<Job> GetByCategoryId(long categoryId)
        {
            return _context.Jobs.Where(j => j.CategoryId == categoryId);
        }
    }
}