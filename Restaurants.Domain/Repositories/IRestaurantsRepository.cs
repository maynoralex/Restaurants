using System.Data.Common;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;

public interface IRestaurantsRepository
{
    Task<IEnumerable<Restaurant>> GetAllAsync();

    Task<Restaurant?> GetByIdAsync(int id);

    Task<int> Create(Restaurant entity);

    Task Delete(Restaurant entity);

    Task SaveChanges(Restaurant entity);

    Task<(IEnumerable<Restaurant>, int)> GetAllMatchingPhraseAsync(string searchPhrase, int pageNumber, int pageSize, string? sortBy, SortDirection sortDirection);
}
