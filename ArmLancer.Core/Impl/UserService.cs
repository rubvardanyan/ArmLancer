using System;
using ArmLancer.Core.Interfaces;
using ArmLancer.Data.Context;
using ArmLancer.Data.Models;

namespace ArmLancer.Core.Impl
{
    public class UserService : CrudService<User>, IUserService
    {
        public UserService(
            IServiceProvider serviceProvider,
            ArmLancerDbContext context) : base(serviceProvider, context)
        {
        }
        
        public bool Exists(string userName, string password)
        {
            throw new System.NotImplementedException();
        }

        public User Register(User user)
        {
            throw new System.NotImplementedException();
        }

        public User GetByCredentials(string userName, string password)
        {
            throw new System.NotImplementedException();
        }
    }
}