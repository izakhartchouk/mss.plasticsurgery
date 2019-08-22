using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MSS.PlasticSurgery.DataAccess.EntityFrameworkCore;

namespace MSS.PlasticSurgery
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            IConfigurationRoot configurationRoot = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{envName ?? "Production"}.json", optional: false)
                .Build();

            var connectionString = configurationRoot.GetConnectionString("DefaultDbConnectionString");

            var dbContextOptionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
                .UseLazyLoadingProxies()
                .UseSqlServer(connectionString);

            return new AppDbContext(dbContextOptionsBuilder.Options);
        }
    }
}
