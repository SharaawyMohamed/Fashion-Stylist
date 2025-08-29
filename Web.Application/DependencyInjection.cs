
using FluentValidation;
using Mapster;
using MapsterMapper;
using MediatR.NotificationPublishers;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Web.APIs.Validators;
using Web.Application.Mapping;
using Web.Application.Service;
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
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(typeof(MappingProfile).Assembly);

            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();

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
            services.AddScoped<IMediaService, MediaService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IPaymopService, PaymopService>();

            services.AddMemoryCache();

            return services;
        }
    }
}
