using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Seawars.Domain.Entities;

namespace Seawars.DAL.Context
{
    public class MsSqlContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Step> Steps { get; set; }
        public MsSqlContext(DbContextOptions<MsSqlContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique();


            builder.Entity<User>()
                .HasMany<Game>()
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.Cascade);



        }
    }
}
