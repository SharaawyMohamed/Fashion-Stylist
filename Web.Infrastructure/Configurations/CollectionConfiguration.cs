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
	public class CollectionConfiguration : IEntityTypeConfiguration<Collection>
	{
		public void Configure(EntityTypeBuilder<Collection> builder)
		{

			builder.HasKey(c => c.id);

			builder.Property(c => c.title)
				.IsRequired()
				.HasMaxLength(200);

			builder.Property(c => c.subTitle)
				.HasMaxLength(200);

			builder.Property(c => c.description)
				.HasMaxLength(2000);  

			builder.Property(c => c.price)
				.HasColumnType("decimal(18,2)")
				.IsRequired();

			builder.Property(c => c.pictureUrl)
				.IsRequired()
				.HasMaxLength(500);

			builder.HasMany(c => c.items)
				.WithOne(i => i.collection)
				.HasForeignKey(i => i.CollectionId);

		}
	}
}
