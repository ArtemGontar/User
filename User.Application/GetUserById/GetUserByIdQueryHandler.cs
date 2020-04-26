using MediatR;
using Shared.Identity;
using Shared.Persistence.MySql;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace User.Application.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ApplicationUser>
    {
        private readonly ISqlRepository<ApplicationUser> _userRepository;
        public GetUserByIdQueryHandler(ISqlRepository<ApplicationUser> userRepository)
        {
            _userRepository = userRepository;
        }
        public Task<ApplicationUser> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
