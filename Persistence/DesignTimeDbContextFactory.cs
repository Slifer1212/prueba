using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Persistence.DbContext;

namespace Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory() , "../API"))
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
        
        var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        optionBuilder.UseSqlServer(connectionString);
        
        return new ApplicationDbContext(optionBuilder.Options);
        
    }
}