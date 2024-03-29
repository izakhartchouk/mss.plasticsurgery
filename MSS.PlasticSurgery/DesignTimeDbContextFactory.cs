﻿using System;
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
            // ASPNETCORE_ENVIRONMENT variable should be set in system environment variables
            // in order to envName was not null, so the migrations could be applied to target environment
            // Remove ASPNETCORE_ENVIRONMENT variable from system environment variables
            // in order to launch the app locally, since hostingEnvironment.EnvironmentName will contain
            // duplicates of environment name, e.g. 'Development;Development'
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            IConfigurationRoot configurationRoot = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{environmentName ?? "Production"}.json", optional: false)
                .Build();

            var connectionString = configurationRoot.GetConnectionString("DefaultDbConnectionString");

            var dbContextOptionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
                .UseLazyLoadingProxies()
                .UseSqlServer(connectionString);

            return new AppDbContext(dbContextOptionsBuilder.Options);
        }
    }
}
