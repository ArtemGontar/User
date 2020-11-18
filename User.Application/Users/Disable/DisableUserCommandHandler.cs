using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Shared.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace User.Application.Users.Disable
{
    public class DisableUserCommandHandler : IRequestHandler<DisableUserCommand, Guid>
    {
        private readonly ILogger<DisableUserCommandHandler> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        public DisableUserCommandHandler(ILogger<DisableUserCommandHandler> logger,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }
        public async Task<Guid> Handle(DisableUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());

            if (user == null)
            {
                _logger.LogError($"User with id {request.UserId} was not found");
                throw new InvalidOperationException($"User with id {request.UserId} was not found");
            }

            await _userManager.SetLockoutEnabledAsync(user, true);
            await _userManager.SetLockoutEndDateAsync(user, DateTime.Today.AddDays(1));

            return user.Id;
        }
    }
}
