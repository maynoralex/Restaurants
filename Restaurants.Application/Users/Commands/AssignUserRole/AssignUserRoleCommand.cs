using MediatR;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Restaurants.Application.Users.Commands.AssignUserRole;

public class AssignUserRoleCommand : IRequest
{
    public string UserEmail { get; set; } = default!;
    public string RoleName { get; set; } = default!;

}
