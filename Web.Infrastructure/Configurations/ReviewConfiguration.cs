using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Domain.Entites;

namespace Web.Infrastructure.Configurations
{
	public class ReviewConfiguration : IEntityTypeConfiguration<Review>
	{
		public void Configure(EntityTypeBuilder<Review> builder)
		{

			builder.HasKey(r => r.id);

			builder.Property(r => r.comment)
				.IsRequired()
				.HasMaxLength(1000); 

			builder.Property(r => r.rate)
				.IsRequired()
				.HasColumnType("int");

			builder.HasOne(r => r.appUser)
				.WithMany()
				.HasForeignKey(r => r.userId);

			builder.HasOne(r => r.product)
				.WithMany(p => p.reviews)
				.HasForeignKey(r => r.productId);

			builder.HasIndex(r => r.productId);
			builder.HasIndex(r => r.userId);

		}
	}
}
