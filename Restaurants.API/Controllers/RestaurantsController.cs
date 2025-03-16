using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants;
using Restaurants.Application.Restaurants.DTOs;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/restaurants")]
public class RestaurantsController(IRestaurantService restaurantService) : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetAll(){
        var restaurants = await restaurantService.GetAllRestaurants();
        return Ok(restaurants);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id){
        var restaurant = await restaurantService.GetById(id);
        if(restaurant is null)
            return NotFound();
        return Ok(restaurant);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRestaurant([FromBody] CreateRestaurantDto newRestaurant){
        int id = await restaurantService.Create(newRestaurant);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }
}
