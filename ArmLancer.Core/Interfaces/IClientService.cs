using ArmLancer.Data.Models;

namespace ArmLancer.Core.Interfaces
{
    public interface IClientService : ICrudService<Client>
    {
        Client GetByUserId(long userId);
    }
}