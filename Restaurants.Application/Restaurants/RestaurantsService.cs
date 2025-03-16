using System.ComponentModel;
using System.Runtime.InteropServices;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.DTOs;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
namespace Restaurants.Application.Restaurants;

internal class RestaurantsService(IRestaurantsRepository restaurantsRepository, 
    ILogger<RestaurantsService> logger,
    IMapper mapper) : IRestaurantService
{
    public async Task<int> Create(CreateRestaurantDto dto)
    {
        var restaurant = mapper.Map<Restaurant>(dto);
        int id = await restaurantsRepository.Create(restaurant);
        return id;
    }

    public async Task<IEnumerable<RestaurantDto>> GetAllRestaurants() {
        logger.LogInformation("Getting all restaurants.");
        var restaurants = await restaurantsRepository.GetAllAsync();
        //changed implementation with using Automapper, to stop coding FromEntity Static Methods
        //var restaurantsDto = restaurants.Select(RestaurantDto.FromEntity); 
        var restaurantsDto = mapper.Map<IEnumerable<RestaurantDto>>(restaurants);
        return restaurantsDto!;
    }

    public async Task<RestaurantDto?> GetById(int id) {
        logger.LogInformation($"Getting restaurant {id}.");
        var restaurant = await restaurantsRepository.GetByIdAsync(id);
        //changed implementation with using Automapper, to stop coding FromEntity Static Methods
        //var restaurantDto = RestaurantDto.FromEntity(restaurant);
        var restaurantDto = mapper.Map<RestaurantDto>(restaurant);
        return restaurantDto;
    }
    
}
