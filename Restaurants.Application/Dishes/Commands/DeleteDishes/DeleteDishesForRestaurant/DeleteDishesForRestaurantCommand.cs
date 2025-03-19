using MediatR;

namespace Restaurants.Application.Dishes.Commands.DeleteDishes.DeleteDishesForRestaurant;

public class DeleteDishesForRestaurantCommand(int restaurantId) : IRequest
{
    public int RestaurantId { get; } = restaurantId;
}
