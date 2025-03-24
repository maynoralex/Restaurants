using AutoMapper;
using FluentAssertions;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.DTOs;
using Restaurants.Domain.Entities;
using Xunit;

namespace Restaurants.Application.Tests.Restaurants.Commands.DTOs;

public class RestaurantsProfileTests
{
    private IMapper _mapper;

    public RestaurantsProfileTests() 
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<RestaurantsProfile>();
        });

        _mapper = configuration.CreateMapper();
        
    }

    [Fact]
    public void CreateMap_ForRestaurantToRestaurantDTO_MapsCorrectly(){
        //arrange
        
        var restaurant = new Restaurant{
            Id = 1,
            Name = "Test restaurant",
            Description = "Test Description",
            Category = "Test Category",
            HasDelivery = true,
            ContactEmail = "Test@Email.com",
            ContactNumber = "12345678",
            Address = new Address{
                City = "Test City",
                Street = "Test Street",
                PostalCode = "223344"
            }
        };

        //Act
        var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);

        //Assert
        restaurantDto.Should().NotBeNull();
        restaurantDto.Id.Should().Be(restaurant.Id);
        restaurantDto.Name.Should().Be(restaurant.Name);
        restaurantDto.Description.Should().Be(restaurant.Description);
        restaurantDto.Category.Should().Be(restaurant.Category);
        restaurantDto.HasDelivery.Should().Be(restaurant.HasDelivery);
        
        restaurantDto.City.Should().Be(restaurant.Address.City);
        restaurantDto.Street.Should().Be(restaurant.Address.Street);
        restaurantDto.PostalCode.Should().Be(restaurant.Address.PostalCode);
        
        
    }

    [Fact]
    public void CreateMap_ForCreateRestaurantCommandToRestaurant_MapsCorrectly()
    {
        //arrange
        
        var command = new CreateRestaurantCommand
        {

            Name = "Test restaurant",
            Description = "Test Description",
            Category = "Test Category",
            HasDelivery = true,
            ContactEmail = "Test@Email.com",
            ContactNumber = "12345678",
            City = "Test City",
            Street = "Test Street",
            PostalCode = "223344"
        };

        //Act
        var restaurant = _mapper.Map<Restaurant>(command);

        //Assert
        restaurant.Should().NotBeNull();
        restaurant.Name.Should().Be(command.Name);
        restaurant.Description.Should().Be(command.Description);
        restaurant.Category.Should().Be(command.Category);
        restaurant.HasDelivery.Should().Be(command.HasDelivery);
        restaurant.ContactEmail.Should().Be(command.ContactEmail);
        restaurant.ContactNumber.Should().Be(command.ContactNumber);
        restaurant.Address.Should().NotBeNull();
        restaurant.Address.City.Should().Be(command.City);
        restaurant.Address.Street.Should().Be(command.Street);
        restaurant.Address.PostalCode.Should().Be(command.PostalCode);

    }

    [Fact]
    public void CreateMap_ForUpdateRestaurantCommandToRestaurant_MapsCorrectly()
    {
        //arrange
        
        var command = new UpdateRestaurantCommand
        {
            Id = 1,
            Name = "Updated restaurant",
            Description = "Updated Description",
            Category = "Updated Category",
            HasDelivery = false
        };

        //Act
        var restaurant = _mapper.Map<Restaurant>(command);

        //Assert
        restaurant.Should().NotBeNull();
        restaurant.Id.Should().Be(command.Id);
        restaurant.Name.Should().Be(command.Name);
        restaurant.Description.Should().Be(command.Description);
        restaurant.Category.Should().Be(command.Category);
        restaurant.HasDelivery.Should().Be(command.HasDelivery);
        

    }
    
}
