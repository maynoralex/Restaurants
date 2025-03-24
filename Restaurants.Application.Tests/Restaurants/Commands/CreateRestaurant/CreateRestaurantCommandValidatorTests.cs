using FluentValidation.TestHelper;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.CreateRestaurant;

namespace Restaurants.Application.Tests.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandValidatorTests
{

    [Fact]
    public void Validator_ForValidCommand_ShouldNotHaveValidationErrors(){
        //arrange
        var command = new CreateRestaurantCommand(){
            Name = "Test",
            Category = "Italian",
            ContactEmail = "test@test.com",
            PostalCode = "12-345"
        };

        var validator = new CreateRestaurantCommandValidator();

        //act
        var result = validator.TestValidate(command);

        //assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validator_ForValidCommand_ShouldHaveValidationErrors(){
        //arrange
        var command = new CreateRestaurantCommand(){
            Name = "Te",
            Category = "Italian",
            ContactEmail = "@test.com",
            PostalCode = "12345"
        };

        var validator = new CreateRestaurantCommandValidator();

        //act
        var result = validator.TestValidate(command);

        //assert
        result.ShouldHaveValidationErrorFor(c => c.Name);
        result.ShouldHaveValidationErrorFor(c => c.ContactEmail);
        result.ShouldHaveValidationErrorFor(c => c.PostalCode);
    }

    [Theory]
    [InlineData("10202")]
    [InlineData("102-30")]
    [InlineData("10 220")]
    [InlineData("10-2 20")]
    public void Validator_ForInvalidPostalCode_ShouldHaveValidationErrorsForPostalCodeProperty(string postalCode)
    {
        //arrange
        var validator = new CreateRestaurantCommandValidator();
        var command = new CreateRestaurantCommand { PostalCode = postalCode };
        //act
        var result = validator.TestValidate(command);
        //assert 
        result.ShouldHaveValidationErrorFor(c => c.PostalCode);
    }

    
}
