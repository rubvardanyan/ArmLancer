using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ArmLancer.Data.Context
{
    public class ArmLancerDbContextFactory : IDesignTimeDbContextFactory<ArmLancerDbContext>
    {
        
        public ArmLancerDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ArmLancerDbContext>();
            builder.UseMySql("Server=localhost;Database=ArmLancerDb;Uid=ArmLancerUser;Pwd=ArmLancerPass;");
            return new ArmLancerDbContext(builder.Options);
        }
    }
}