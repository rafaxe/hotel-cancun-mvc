using HotelCancun.Business.Interfaces;
using HotelCancun.Business.Notifications;
using HotelCancun.Business.Services;
using HotelCancun.Data.Context;
using HotelCancun.Data.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace HotelCancun.Api.Configurations
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

            services.AddScoped<INotifier, Notifier>();
            services.AddScoped<IHotelService, HotelService>();
            services.AddScoped<ISuiteService, SuiteService>();
            services.AddScoped<IReservationService, ReservationService>();

            return services;
        }
    }
}