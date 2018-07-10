using System;
using System.Collections.Generic;
using System.Linq;
using ArmLancer.Core.Interfaces;
using ArmLancer.Data.Context;
using ArmLancer.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ArmLancer.Core.Impl
{
    public class FavoriteService : CrudService<Favorite>, IFavoriteService
    {
        public FavoriteService(IServiceProvider serviceProvider, ArmLancerDbContext context) : base(serviceProvider, context)
        {
        }

        public bool DoesFreelancerOwnFavorite(long clientId, long favoriteId)
        {
            return _context.Favorites.Any(f => f.Id == favoriteId && f.ClientId == clientId);
        }

        public IEnumerable<Favorite> GetByClientId(long clientId)
        {
            return _context.Favorites.Where(f => f.ClientId == clientId).AsNoTracking();
        }
    }
}