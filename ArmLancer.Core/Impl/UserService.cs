using System;
using System.Linq;
using ArmLancer.Core.Utils.Helpers;
using ArmLancer.Core.Interfaces;
using ArmLancer.Data.Context;
using ArmLancer.Data.Models;
using Microsoft.EntityFrameworkCore;

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
            return null == GetByCredentials(userName, password);
        }

        public User Register(User user)
        {
            Create(user);
            return user;
        }

        public User GetByCredentials(string userName, string password)
        {
            return _context.Users.Include(u => u.Client).SingleOrDefault(u =>
                u.UserName == userName && u.Password == CryptoHelper.Encrypt(password));
        }

        public User GetByUserName(string userName)
        {
            return _context.Users.Include(u => u.Client).SingleOrDefault(u => u.UserName == userName);
        }
    }
}