using ArmLancer.Data.Models;

namespace ArmLancer.Core.Interfaces
{
    public interface IRatingService : ICrudService<Rating>
    {
        bool FreeLancerCanWriteReview(long jobId, long clientIdFrom);
        bool EmployeerCanWriteReview(long jobId, long clientIdFrom);
    }
}