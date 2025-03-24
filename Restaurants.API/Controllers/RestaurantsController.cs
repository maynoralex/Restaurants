using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Common;
using Restaurants.Application.Restaurants;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.DTOs;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;
using Restaurants.Infrastructure.Authorization;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/restaurants")]
[Authorize]
public class RestaurantsController(IMediator mediator) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<PagedResult<RestaurantDto>>> GetAll([FromQuery] GetAllRestaurantsQuery query){
        var restaurants = await mediator.Send(query);
        return Ok(restaurants);
    }

    [HttpGet("{id}")]
    [Authorize (Policy = PolicyNames.HasNationality)]
    public async Task<ActionResult<RestaurantDto>> GetById([FromRoute] int id){
        var restaurant = await mediator.Send(new GetRestaurantByIdQuery(id));
        return Ok(restaurant);
    }

    [HttpDelete("{id}")]    
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRestaurant([FromRoute] int id){
        await mediator.Send(new DeleteRestaurantCommand(id));
        return NoContent();
    }

    [HttpPatch("{id}")]    
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateRestaurant([FromRoute] int id, [FromBody] UpdateRestaurantCommand editedRestaurant) {
        editedRestaurant.Id = id;
        await mediator.Send(editedRestaurant);
        return NoContent();
        
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> CreateRestaurant([FromBody] CreateRestaurantCommand newRestaurant){

        int id = await mediator.Send(newRestaurant);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }
}
