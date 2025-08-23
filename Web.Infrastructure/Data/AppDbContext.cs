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
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Chat> Chats { get; set; }
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
            builder.Entity<ChatMessage>()
              .HasIndex(c => new { c.SenderUserId, c.ReceiverUserId, c.CreatedAt });

            builder.Entity<ChatMessage>()
             .HasIndex(c => new { c.ReceiverUserId, c.SenderUserId, c.CreatedAt });

            builder.Entity<Chat>(entity =>
            {

                entity.HasIndex(c => new { c.FirstUserId, c.SecondUserId })
                      .IsUnique();


                entity.HasMany(c => c.Messages)
                      .WithOne(m => m.Chat)
                      .HasForeignKey(m => m.ChatId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
	}
}
