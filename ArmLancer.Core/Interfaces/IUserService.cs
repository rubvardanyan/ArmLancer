using ArmLancer.Data.Models;

namespace ArmLancer.Core.Interfaces
{
    public interface IUserService : ICrudService<User>
    {
        bool Exists(string userName, string password);
        User Register(User user);
        User GetByCredentials(string userName, string password);
        User GetByUserName(string userName);
    }
}