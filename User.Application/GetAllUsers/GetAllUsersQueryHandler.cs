using MediatR;
using Shared.Identity;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Shared.Persistence.MySql;

namespace User.Application.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<ApplicationUser>>
    {
        private readonly ISqlRepository<ApplicationUser> _userRepository;

        public GetAllUsersQueryHandler(ISqlRepository<ApplicationUser> userRepository)
        {
            _userRepository = userRepository;
        }
        public Task<IEnumerable<ApplicationUser>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
