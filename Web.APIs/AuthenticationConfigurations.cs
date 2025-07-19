using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.APIs.Validators;

namespace Web.APIs
{
	public static class AuthenticationConfigurations
	{
		public static IServiceCollection AddJWTConfigurations(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(o =>
			{
				o.RequireHttpsMetadata = false;
				o.SaveToken = false;
				o.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:Key"]!)),

					ValidateIssuer = true,
					ValidIssuer = configuration["Token:Issuer"],

					ValidateAudience = true,
					ValidAudience = configuration["Token:Audience"],

					ValidateLifetime = true,
					ClockSkew = TimeSpan.Zero
				};

			});
			return services;
		}
	}
}
