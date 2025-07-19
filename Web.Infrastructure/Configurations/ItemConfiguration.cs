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
	public class ItemConfiguration : IEntityTypeConfiguration<Item>
	{
		public void Configure(EntityTypeBuilder<Item> builder)
		{

			builder.HasKey(i => i.id);

			builder.Property(i => i.title)  
				.IsRequired()
				.HasMaxLength(200);

			builder.Property(i => i.pictureUrl)
				.IsRequired()
				.HasMaxLength(500);

			builder.Property(i => i.description)
				.HasMaxLength(2000);

			builder.HasOne(i => i.collection)
				.WithMany(c => c.items)
				.HasForeignKey(i => i.CollectionId);

			builder.HasIndex(i => i.title);
			builder.HasIndex(i => i.CollectionId);

		}
	}
}
