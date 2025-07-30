using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.Entites;

namespace Web.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Collection> collections { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Favorite> Favorites { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
		{
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
			base.OnModelCreating(builder);

            builder.Entity<Favorite>()
.HasKey(f => new { f.UserId, f.ProductId });
            ////////////////
            builder.Entity<Favorite>()
          .HasOne(f => f.User)
          .WithMany(f => f.Favorites)
          .HasForeignKey(f => f.UserId)
           .OnDelete(DeleteBehavior.Restrict);
            //////////////
            builder.Entity<Favorite>()
                .HasOne(f => f.Product)
                .WithMany()
                .HasForeignKey(f => f.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
	}
}
