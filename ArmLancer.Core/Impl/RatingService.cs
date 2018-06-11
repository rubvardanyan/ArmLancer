using System;
using ArmLancer.Core.Interfaces;
using ArmLancer.Data.Context;
using ArmLancer.Data.Models;

namespace ArmLancer.Core.Impl
{
    public class RatingService : CrudService<Rating>, IRatingService
    {
        public RatingService(IServiceProvider serviceProvider, ArmLancerDbContext context) : base(serviceProvider, context)
        {
        }
    }
}