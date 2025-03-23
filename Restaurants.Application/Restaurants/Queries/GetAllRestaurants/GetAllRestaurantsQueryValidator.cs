using FluentValidation;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryValidator : AbstractValidator<GetAllRestaurantsQuery>
{
    private int[] validPageSizes = [5,10,15,20,25,30];   
    private string[] allowedSortColumnNames = [nameof(Restaurant.Name)
        , nameof(Restaurant.Description)
        , nameof(Restaurant.Category)]; 
    public GetAllRestaurantsQueryValidator()
    {
        RuleFor(r => r.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(r => r.PageSize)
            .Must(value => validPageSizes.Contains(value))
            .WithMessage($"Page size must be in [{string.Join(",", validPageSizes)}]");

        RuleFor(r => r.SortBy)
            .Must(value => allowedSortColumnNames.Contains(value))
            .When(q => q.SortBy != null)
            .WithMessage($"Sort by is optional, or must be in [{string.Join(",", allowedSortColumnNames)}]");
        
    }

}
