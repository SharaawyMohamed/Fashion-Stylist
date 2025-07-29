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
	public class CategoryConfiguration : IEntityTypeConfiguration<Category>
	{
		public void Configure(EntityTypeBuilder<Category> builder)
		{

			builder.HasKey(c => c.id); 

			builder.Property(c => c.Name)
				.IsRequired()
				.HasMaxLength(100);  

			builder.Property(c => c.PictureUrl)
				.HasMaxLength(500);

			builder.HasMany(c => c.Products)
				.WithOne(p => p.category)
				.HasForeignKey(p => p.categoryId);

			builder.HasIndex(c => c.Name)
				.IsUnique();  

			
		}
	}
}
