
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Web.Domain.DTOs.EmailDTO;
using Web.Domain.Entites;
using Web.Infrastructure.Data;

namespace Web.Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
		{

			services.AddDbContext<AppDbContext>(options =>
			{
				options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
				
			});

			services.AddIdentity<AppUser, IdentityRole>()
				.AddEntityFrameworkStores<AppDbContext>()
				.AddDefaultTokenProviders();

			services.Configure<EmailDto>(config.GetSection("MailSettings"));

			
			return services;
		}
	}
}
