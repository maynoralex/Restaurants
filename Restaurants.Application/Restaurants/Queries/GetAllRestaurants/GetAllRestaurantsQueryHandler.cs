using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Common;
using Restaurants.Application.Restaurants.DTOs;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryHandler(IRestaurantsRepository restaurantsRepository, 
    ILogger<GetAllRestaurantsQueryHandler> logger,
    IMapper mapper) : IRequestHandler<GetAllRestaurantsQuery, PagedResult<RestaurantDto>>
{
    public async Task<PagedResult<RestaurantDto>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all restaurants.");
        var (restaurants, totalCount) = await restaurantsRepository.GetAllMatchingPhraseAsync(request.SearchPhrase!
            ,request.PageNumber
            ,request.PageSize
            ,request.SortBy
            ,request.SortDirection);
        var restaurantsDto = mapper.Map<IEnumerable<RestaurantDto>>(restaurants);

        var result = new PagedResult<RestaurantDto>(restaurantsDto, totalCount, request.PageSize, request.PageNumber);
        

        return result;
    }
}
