using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Shahrah.Transporter.Infrastructure.Persistence;

public class DataContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(x => x.UseNetTopologySuite())
            .Options;

        return new ApplicationDbContext(options);
    }
}