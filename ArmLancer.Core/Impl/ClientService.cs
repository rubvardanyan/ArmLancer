using System;
using System.Linq;
using ArmLancer.Core.Interfaces;
using ArmLancer.Data.Context;
using ArmLancer.Data.Models;

namespace ArmLancer.Core.Impl
{
    public class ClientService : CrudService<Client>, IClientService
    {
        public ClientService(IServiceProvider serviceProvider, ArmLancerDbContext context) : base(serviceProvider, context)
        {
        }

        public Client GetByUserId(long userId)
        {
            return _context.Clients.SingleOrDefault(c => c.UserId == userId);
        }
    }
}