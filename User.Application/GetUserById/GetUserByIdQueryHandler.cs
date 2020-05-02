using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace User.Application.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ApplicationUser>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        public GetUserByIdQueryHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<ApplicationUser> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());

            if (user == null)
                throw new InvalidOperationException(nameof(user));

            return user;
        }
    }
}
