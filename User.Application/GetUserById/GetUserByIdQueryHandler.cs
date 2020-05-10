using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Shared.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace User.Application.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ApplicationUser>
    {
        private readonly ILogger<GetUserByIdQueryHandler> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        public GetUserByIdQueryHandler(ILogger<GetUserByIdQueryHandler> logger, 
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }
        public async Task<ApplicationUser> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());

            if (user == null)
            {
                _logger.LogError($"User with id {request.UserId} was not found");
                throw new InvalidOperationException($"User with id {request.UserId} was not found");
            }

            return user;
        }
    }
}
