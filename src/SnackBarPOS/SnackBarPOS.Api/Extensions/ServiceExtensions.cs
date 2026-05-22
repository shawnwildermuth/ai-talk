using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SnackBarPOS.Api.Data;
using SnackBarPOS.Api.Data.Repositories;
using SnackBarPOS.Api.Mappings;

namespace SnackBarPOS.Api.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        // Database
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection"),
                sql => sql.EnableRetryOnFailure()));

        // Repositories
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        // FluentValidation - register all validators from assembly
        services.AddValidatorsFromAssemblyContaining<Program>();

        // Mapster configuration
        MappingConfig.Configure();

        return services;
    }
}
