using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using Restaurants.Application.Restaurants.DTOs;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Xunit;
namespace Restaurants.API.Tests.Controllers;

public class RestaurantsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock = new();
    public RestaurantsControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder => {
            builder.ConfigureTestServices(services => 
            {
                services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                services.Replace(ServiceDescriptor.Scoped(typeof(IRestaurantsRepository),
                                                        _ => _restaurantsRepositoryMock.Object));
                
            });
        });
    }

    [Fact]
    public async Task GetAll_ForValidRequest_Returns200OK()
    {
        //Arrange
        var client = _factory.CreateClient();

        //act
        var result = await client.GetAsync("/api/restaurants?pageNumber=1&pageSize=10");

        //assert
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

     [Fact]
    public async Task GetAll_ForInvalidRequest_Returns400BadRequest()
    {
        //Arrange
        var client = _factory.CreateClient();

        //act
        var result = await client.GetAsync("/api/restaurants");

        //assert
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetById_ForExistingId_ShouldReturn200Ok()
    {
        //Arrange
        var client = _factory.CreateClient();
        var id = 99;
        var restaurant = new Restaurant{
            Id = id,
            Name = "Test",
            Description = "Test Description",
            Category = "Test Category"
        };

        _restaurantsRepositoryMock.Setup(m => m.GetByIdAsync(id)).ReturnsAsync(restaurant);

        //
        //act
        var response = await client.GetAsync($"/api/restaurants/{id}");
        var restaurantDto = await response.Content.ReadFromJsonAsync<RestaurantDto>();

        //assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        restaurantDto.Should().NotBeNull();
        restaurantDto!.Id.Should().Be(restaurant.Id);
        restaurantDto.Name.Should().Be(restaurant.Name);
        restaurantDto.Description.Should().Be(restaurant.Description);
        restaurantDto.Category.Should().Be(restaurant.Category);
    }

    [Fact]
    public async Task GetById_ForNonExistingId_Returns404NotFound()
    {
        //Arrange
        var client = _factory.CreateClient();
        var id = 1123;

        _restaurantsRepositoryMock.Setup(m => m.GetByIdAsync(id)).ReturnsAsync((Restaurant?)null);

        //
        //act
        var result = await client.GetAsync($"/api/restaurants/{id}");

        //assert
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

}
