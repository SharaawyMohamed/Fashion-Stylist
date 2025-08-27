
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
using Microsoft.AspNetCore.Builder.Extensions;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Web.Application.Hubs;
using Microsoft.AspNetCore.SignalR;
using Web.Infrastructure.Repositories;

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
            builder.Services.AddHttpClient();


            builder.Services.AddInfrastructure(builder.Configuration)
				.AddJWTConfigurations(builder.Configuration)
				.AddApplication();
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "e-fashion-f1215-firebase-adminsdk-fbsvc-3e488f2626.json")),
            });
            builder.Services.AddSignalR();

            builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials()
                          .SetIsOriginAllowed(_ => true);
                });
            });

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
                app.UseCors();
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
            app.MapHub<ChatHub>("/chatHub");
            app.MapControllers();

			app.Run();
		}
	}
}
