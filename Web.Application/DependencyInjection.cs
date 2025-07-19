
using FluentValidation;
using Mapster;
using MediatR.NotificationPublishers;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Web.APIs.Validators;
using Web.Application.Mapping;
using Web.Application.Service;
using Web.Domain.DTOs.AccountDTO;
using Web.Domain.Interfaces;
using Web.Domain.Repositories;
using Web.Infrastructure.Repositories;
using Web.Infrastructure.Service;

namespace Web.Application
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddApplication(this IServiceCollection services)
		{
			services.AddMapster();
			services.AddAutoMapper(typeof(MappingProfile).Assembly);
			services.AddValidatorsFromAssemblyContaining<RegisterDTOValidator>();

			services.AddMediatR(cfg =>
			{
				cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
				cfg.NotificationPublisher = new TaskWhenAllPublisher();
			});

			services.AddScoped<IUnitOfWork, UnitOfWrok>();
			services.AddScoped<IAccountService, AccountService>();
			services.AddScoped<IAuthService, AuthService>();
			services.AddScoped<IMediaService,MediaService>();
			services.AddTransient<IAuthService, AuthService>();
			services.AddTransient<IEmailService, EmailService>();
			services.AddMemoryCache();

			return services;
		}
	}
}
