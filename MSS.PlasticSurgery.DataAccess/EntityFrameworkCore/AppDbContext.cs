using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MSS.PlasticSurgery.DataAccess.Entities;

namespace MSS.PlasticSurgery.DataAccess.EntityFrameworkCore
{
    public class AppDbContext : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Operation> Operations { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Comment> Comments { get; set; }
    }
}