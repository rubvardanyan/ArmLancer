using System.Collections.Generic;
using ArmLancer.Data.Models;

namespace ArmLancer.Core.Interfaces
{
    public interface IFavoriteService : ICrudService<Favorite>
    {
        bool DoesFreelancerOwnFavorite(long clientId, long favoriteId);
        IEnumerable<Favorite> GetByClientId(long clientId);
    }
}