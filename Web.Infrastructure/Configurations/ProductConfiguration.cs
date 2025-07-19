using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.Entites;

namespace Web.Infrastructure.Configurations
{
	internal class ProductConfiguration : IEntityTypeConfiguration<Product>
	{
		public void Configure(EntityTypeBuilder<Product> builder)
		{
			
			builder.HasKey(p => p.id);

			builder.Property(p => p.title)
				.IsRequired()
				.HasMaxLength(200);

			builder.Property(p => p.pictureUrl)
				.IsRequired()
				.HasMaxLength(500);

			builder.Property(p => p.basePrice)
				.IsRequired()
				.HasColumnType("decimal(18,2)");

			builder.Property(p => p.discountedPrice)
				.IsRequired()
				.HasColumnType("decimal(18,2)");

			builder.Property(p => p.rate)
				.HasColumnType("float")
				.HasDefaultValue(0f);

			builder.Property(p => p.brand);

			builder.Property(p => p.colors)
				.IsRequired();

			builder.Property(p => p.sizes)
				.IsRequired();

			builder.HasMany(p => p.reviews)
				.WithOne(r => r.product)
				.HasForeignKey(r => r.productId);

			builder.HasIndex(p => p.brand);
			builder.HasIndex(p => p.basePrice);

		}
	}
}
