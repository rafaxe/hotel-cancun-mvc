using Cancun.App.Extensions;
using Cancun.Business.Intefaces;
using Cancun.Business.Notifications;
using Cancun.Business.Services;
using Cancun.Data.Context;
using Cancun.Data.Repository;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;

namespace Cancun.App.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<AppDbContext>();
            services.AddScoped<ISuiteRepository, SuiteRepository>();
            services.AddScoped<IHotelRepository, HotelRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddSingleton<IValidationAttributeAdapterProvider, CurrencyValidationAttributeAdapterProvider>();

            services.AddScoped<INotifier, Notifier>();
            services.AddScoped<IHotelService, HotelService>();
            services.AddScoped<ISuiteService, SuiteService>();
            services.AddScoped<IReservationService, ReservationService>();

            return services;
        }
    }
}