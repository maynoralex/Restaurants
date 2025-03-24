using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Xunit;
using FluentAssertions;
namespace Restaurants.Application.Tests.Users;

public class CurrentUserTests
{
    [Theory()]
    [InlineData(UserRoles.Admin)]
    [InlineData(UserRoles.User)]
    public void IsInRole_WithMatchingRole_ReturnsTrue(string roleName)
    {
        // Arrange
        var currentUser = new CurrentUser("1", "test@example.com", [UserRoles.User, UserRoles.Admin], null, null);

        // Act
        var isInRole = currentUser.IsInRole(roleName);

        // Assert
        isInRole.Should().BeTrue();

    }

    [Fact()]
    public void IsInRole_WithNoMatchingRole_ReturnsFalse()
    {
        // Arrange
        var currentUser = new CurrentUser("1", "test@example.com", [UserRoles.User, UserRoles.Admin], null, null);

        // Act
        var isInRole = currentUser.IsInRole(UserRoles.Owner);

        // Assert
        isInRole.Should().BeFalse();

    }

    [Fact()]
    public void IsInRole_WithNoMatchingRoleCase_ReturnsFalse()
    {
        // Arrange
        var currentUser = new CurrentUser("1", "test@example.com", [UserRoles.User, UserRoles.Admin], null, null);

        // Act
        var isInRole = currentUser.IsInRole(UserRoles.Admin.ToLower());

        // Assert
        isInRole.Should().BeFalse();

    }
}
