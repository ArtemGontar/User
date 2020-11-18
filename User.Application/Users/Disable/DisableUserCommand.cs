using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace User.Application.Users.Disable
{
    public class DisableUserCommand : IRequest<Guid>
    {
        public Guid UserId { get; set; }
    }
}
