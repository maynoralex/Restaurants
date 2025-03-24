using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Interfaces;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;
using Xunit;
namespace Restaurants.Application.Tests.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantHandlerTests
{
    private readonly Mock<ILogger<UpdateRestaurantCommandHandler>> _loggerMock;
    private readonly Mock<IRestaurantsRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IRestaurantAuthorizationService> _requestAuthoriztionServiceMock;
    
    private readonly UpdateRestaurantCommandHandler _handler;

    public UpdateRestaurantHandlerTests()
    {
        _loggerMock = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
        _repositoryMock = new Mock<IRestaurantsRepository>();
        _mapperMock = new Mock<IMapper>();
        _requestAuthoriztionServiceMock = new Mock<IRestaurantAuthorizationService>();

        _handler = new UpdateRestaurantCommandHandler(_loggerMock.Object,
            _repositoryMock.Object,
            _mapperMock.Object,
            _requestAuthoriztionServiceMock.Object);

    }

    [Fact]
    public async Task Handler_ForValidCommand_ShouldUpdateRestaurant()
    {
        // Arrange
        var restaurantId = 1;
        var command = new UpdateRestaurantCommand(){
            Id = restaurantId,
            Name = "New Test",
            Description = "New Description",
            Category = "New Category",
            HasDelivery = true
        };
        var restaurant = new Restaurant(){
            Id = restaurantId,
            Name = "Test",
            Description = "Description",
            Category = "Category"
        };
        
        _repositoryMock.Setup(r => r.GetByIdAsync(restaurantId)).ReturnsAsync(restaurant);
        _requestAuthoriztionServiceMock.Setup(r => r.Authorize(restaurant, Domain.Constants.ResourceOperation.Update))
            .Returns(true);

        //_mapperMock.Setup(m => m.Map(command, restaurant)).Returns(restaurant); 
        
        // Act
        await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        _repositoryMock.Verify(r => r.SaveChanges(restaurant), Times.Once);
        _mapperMock.Verify(m => m.Map(command, restaurant), Times.Once);
        
    }

    [Fact]
    public void Handler_WithNoExistingRestaurant_ShouldThrowNotFoundException()
    {
        // Arrange
        var restaurantId = 1;
        var command = new UpdateRestaurantCommand(){
            Id = restaurantId
        };

        _repositoryMock.Setup(r => r.GetByIdAsync(restaurantId))
            .ReturnsAsync((Restaurant?)null);

        // Act
        Func<Task> act = async() => await _handler.Handle(command, CancellationToken.None);

        // Assert
        act.ShouldThrow<NotFoundException>()
            .WithMessage($"Restaurant with id: {restaurantId} doesn't exist.");
        
    }

    [Fact]
    public void Handler_WithUnauthorizedUser_ShouldThrowForbidException()
    {
        // Arrange
        var restaurantId = 1;
        var command = new UpdateRestaurantCommand(){
            Id = restaurantId
        };
        var restaurant = new Restaurant();
        
        _repositoryMock.Setup(r => r.GetByIdAsync(restaurantId)).ReturnsAsync(restaurant);
        _requestAuthoriztionServiceMock.Setup(r => r.Authorize(restaurant, Domain.Constants.ResourceOperation.Update))
            .Returns(false);

        // Act
        Func<Task> act = async() => await _handler.Handle(command, CancellationToken.None);

        // Assert
        act.ShouldThrow<ForbidException>();
        
    }

    

}
