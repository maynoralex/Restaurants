using Restaurants.Application.Restaurants.DTOs;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants;

public interface IRestaurantService
{
    Task<IEnumerable<RestaurantDto>> GetAllRestaurants();

    Task<RestaurantDto?> GetById(int id);
    
    Task<int> Create(CreateRestaurantDto newRestaurant);
}
