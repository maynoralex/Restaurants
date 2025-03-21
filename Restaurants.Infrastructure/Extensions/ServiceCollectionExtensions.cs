using Microsoft.Extensions.DependencyInjection;
using Restaurants.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Restaurants.Infrastructure.Seeders;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Repositories;
using Restaurants.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Restaurants.Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;
using Restaurants.Infrastructure.Authorization.Requirements;

namespace Restaurants.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration){
        var connectionString = configuration.GetConnectionString("RestaurantsDb");
        services.AddDbContext<RestaurantsDbContext>(options => options.UseNpgsql(connectionString).EnableSensitiveDataLogging());
        
        services.AddIdentityApiEndpoints<User>()
            .AddRoles<IdentityRole>()
            .AddClaimsPrincipalFactory<RestaurantsUserClaimsPrincipalFactory>()
            .AddEntityFrameworkStores<RestaurantsDbContext>();
        
        services.AddScoped<IRestaurantSeeder, RestaurantSeeder>();
        services.AddScoped<IRestaurantsRepository, RestaurantsRepository>();
        services.AddScoped<IDishesRepository, DishesRepository>();
        services.AddAuthorizationBuilder()
            .AddPolicy(PolicyNames.HasNationality, builder => builder.RequireClaim(AppClaimTypes.Nationality, "German", "Polish"))
            .AddPolicy(PolicyNames.AtLeast20, builder => builder.AddRequirements(new MinimumAgeRequirement(20)));

        services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();

    }

}
