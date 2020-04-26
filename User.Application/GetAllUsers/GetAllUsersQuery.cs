using MediatR;
using Shared.Identity;
using System.Collections.Generic;

namespace User.Application.GetAllUsers
{
    public class GetAllUsersQuery : IRequest<IEnumerable<ApplicationUser>>
    {
    }
}
