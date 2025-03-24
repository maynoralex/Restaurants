using Xunit;
using FluentAssertions;
using Moq;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Restaurants.Domain.Constants;
using Restaurants.Application.Users;
namespace Restaurants.Application.Tests.Users;

public class UserContextTests
{
    [Fact]
    public void GetCurrentUser_WithAuthenticatedUser_ShouldReturnCurrentUser()
    {
        // Arrange
        var dateOfBirth = new DateOnly(1990,1,1);
        var httpContextAccesorMock = new Mock<IHttpContextAccessor>();
        var claims = new List<Claim>(){
            new(ClaimTypes.NameIdentifier, "1"),
            new(ClaimTypes.Email, "test@test.com"),
            new(ClaimTypes.Role, UserRoles.Admin),
            new(ClaimTypes.Role, UserRoles.User),
            new("Nationality", "German"),
            new("DateOfBirth", dateOfBirth.ToString("yyyy-MM-dd"))
        };

        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"));
        httpContextAccesorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext(){
            User = user
        });

        var userContext = new UserContext(httpContextAccesorMock.Object);
        
    
        // Act
        var currentUser = userContext.GetCurrentUser();

        // Assert
        currentUser.Should().NotBeNull();
        currentUser.Id.Should().Be("1");
        currentUser.Email.Should().Be("test@test.com");
        currentUser.Roles.Should().ContainInOrder(UserRoles.Admin, UserRoles.User);
        currentUser.Nationality.Should().Be("German");
        currentUser.DateOfBirth.Should().Be(dateOfBirth);
    }

    [Fact]
    public void GetCurrentUser_WithUserContextNotPresent_ThrowsInvalidOperationException(){
        //arrange
        var httpContextAccesorMock = new Mock<IHttpContextAccessor>();
        httpContextAccesorMock.Setup(x => x.HttpContext).Returns((HttpContext) null);

        var userContext = new UserContext(httpContextAccesorMock.Object);

        //act
        Action action = () => userContext.GetCurrentUser();

        //assert
        action.ShouldThrow<InvalidOperationException>()
            .WithMessage("User Context is not present.");
    }
}
