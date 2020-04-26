using MediatR;
using Shared.Identity;
using System;

namespace User.Application.GetUserById
{
    public class GetUserByIdQuery : IRequest<ApplicationUser>
    {
        public Guid UserId { get; set; }
    }
}
