using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
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
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<CartItem> CartItems { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
		{
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
			base.OnModelCreating(builder);

            builder.Entity<Cart>()
              .HasMany(c => c.Items)
              .WithOne(ci => ci.Cart)
              .HasForeignKey(ci => ci.CartId);

            builder.Entity<CartItem>()
               .HasKey(ci => new { ci.CartId, ci.ProductId });

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
