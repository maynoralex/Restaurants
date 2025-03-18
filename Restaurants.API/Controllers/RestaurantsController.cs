using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.DTOs;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/restaurants")]
public class RestaurantsController(IMediator mediator) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAll(){
        var restaurants = await mediator.Send(new GetAllRestaurantsQuery());
        return Ok(restaurants);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RestaurantDto?>> GetById([FromRoute] int id){
        var restaurant = await mediator.Send(new GetRestaurantByIdQuery(id));
        if(restaurant is null)
            return NotFound();
        return Ok(restaurant);
    }

    [HttpDelete("{id}")]    
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    
    public async Task<IActionResult> DeleteRestaurant([FromRoute] int id){
        var isDeleted = await mediator.Send(new DeleteRestaurantCommand(id));
        if(isDeleted)
            return NoContent();
        return NotFound();
    }

    [HttpPatch("{id}")]    
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateRestaurant([FromRoute] int id, [FromBody] UpdateRestaurantCommand editedRestaurant) {
        editedRestaurant.Id = id;
        var isUpdated = await mediator.Send(editedRestaurant);
        if(isUpdated)
            return NoContent();
        return NotFound();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateRestaurant([FromBody] CreateRestaurantCommand newRestaurant){
        int id = await mediator.Send(newRestaurant);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }
}
