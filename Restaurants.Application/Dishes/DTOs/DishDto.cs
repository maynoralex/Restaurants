using System.Reflection.Metadata.Ecma335;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Dishes.DTOs;

public class DishDto
{

    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Price { get; set; }
    public int? kiloCalories { get; set; }
    public int RestaurantId { get; set; }

    // public static  DishDto FromEntity(Dish dish) {
    //     return new DishDto () {
    //         Id = dish.Id,
    //         Name = dish.Name,
    //         Description = dish.Description,
    //         Price = dish.Price,
    //         kiloCalories = dish.kiloCalories,
    //         RestaurantId = dish.RestaurantId
    //     };

    // }

}
