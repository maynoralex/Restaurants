using AutoMapper;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Interfaces;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandHandler(ILogger<UpdateRestaurantCommandHandler> logger,
    IRestaurantsRepository restaurantsRepository,
    IMapper mapper,
    IRestaurantAuthorizationService authorizationService
) : IRequestHandler<UpdateRestaurantCommand>
{
    public async Task Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating restaurant with id: {RestaurantId} with {@UpdatedRestaurant}.",request.Id, request);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.Id);
        
        if(restaurant == null)
            throw new NotFoundException(nameof(Restaurant), request.Id.ToString());

        if(!authorizationService.Authorize(restaurant, ResourceOperation.Update))
            throw new ForbidException();
        
        mapper.Map(request, restaurant);
        // restaurant.Name = request.Name;
        // restaurant.Description = request.Description;
        // restaurant.HasDelivery = request.HasDelivery;
        // restaurant.Category = request.Category;
        await restaurantsRepository.SaveChanges(restaurant);

        
    }
}
