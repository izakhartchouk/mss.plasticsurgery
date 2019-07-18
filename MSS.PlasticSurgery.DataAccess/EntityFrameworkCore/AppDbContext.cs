using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MSS.PlasticSurgery.DataAccess.Entities;

namespace MSS.PlasticSurgery.DataAccess.EntityFrameworkCore
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Operation> Operations { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Comment> Comments { get; set; }
    }
}