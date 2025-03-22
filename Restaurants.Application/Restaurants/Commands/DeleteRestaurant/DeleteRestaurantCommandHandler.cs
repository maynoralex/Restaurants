using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Interfaces;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant;

public class DeleteRestaurantCommandHandler (ILogger<DeleteRestaurantCommandHandler> logger,
    IRestaurantsRepository restaurantsRepository,
    IRestaurantAuthorizationService authorizationService) : IRequestHandler<DeleteRestaurantCommand>
{
    public async Task Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting restaurant with id: {RestaurantID}.",request.Id);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.Id);
        
        if(restaurant == null)
            throw new NotFoundException(nameof(Restaurant), request.Id.ToString());

        if(!authorizationService.Authorize(restaurant, ResourceOperation.Delete))
            throw new ForbidException();
            
        
        await restaurantsRepository.Delete(restaurant);
        
    }
}
