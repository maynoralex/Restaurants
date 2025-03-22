using Microsoft.Extensions.Logging;
using Restaurants.Application.Interfaces;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;

namespace Restaurants.Infrastructure.Authorization.Services;

public class RestaurantAuthorizationService(ILogger<RestaurantAuthorizationService> logger,
    IUserContext userContext) : IRestaurantAuthorizationService
{
    public bool Authorize(Restaurant restaurant, ResourceOperation resourceOperation)
    {
        var currentUser = userContext.GetCurrentUser();

        logger.LogInformation("Authorizing user {UserEmail}, to {Operation}  for restaurant {RestsaurantName}",
            currentUser!.Email,
            resourceOperation,
            restaurant.Name
        );

        if (resourceOperation == ResourceOperation.Read || resourceOperation == ResourceOperation.Create)
        {
            logger.LogInformation("Create/read operation - Successful authorization. ");
            return true;
        }
        if (resourceOperation == ResourceOperation.Delete && currentUser.IsInRole(UserRoles.Admin))
        {
            logger.LogInformation("Admin user, delete operation - Successful authorization. ");
            return true;
        }
        if ((resourceOperation == ResourceOperation.Update || resourceOperation == ResourceOperation.Delete) && restaurant.OwnerId == currentUser.Id)
        {
            logger.LogInformation("Restaurant owner - successful authorization");
            return true;
        }

        return false;
    }
}
