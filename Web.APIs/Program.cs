
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Web.APIs.Validators;
using Web.Application;
using Web.Application.Mapping;
using Web.Domain.Entites;
using Web.Infrastructure;
using Web.Infrastructure.Data;
using Web.Infrastructure.Service;
using Web.Domain.DTOs.AccountDTO;

namespace Web.APIs
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);


			builder.Services.AddControllers();

			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			builder.Services.AddHttpContextAccessor();

			builder.Services.AddInfrastructure(builder.Configuration)
				.AddJWTConfigurations(builder.Configuration)
				.AddApplication();


			var app = builder.Build();
			if (app.Environment.IsDevelopment())
			{
				using (var scope = app.Services.CreateScope())
				{
					var services = scope.ServiceProvider;
					await IdentitySeeder.SeedAdminAsync(services);
				}
			}

			//if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();

                app.Use(async (ctx, next) =>
                {
                    if (ctx.Request.Path == "/")
                    {
                        ctx.Response.Redirect("/swagger/index.html");
                        return;
                    }
                    await next();
                });
            }

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
