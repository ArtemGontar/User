using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Shared.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace User.Application.Update
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Guid>
    {
        private readonly ILogger<UpdateUserCommandHandler> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        public UpdateUserCommandHandler(ILogger<UpdateUserCommandHandler> logger,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<Guid> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());

            if (user == null)
            {
                _logger.LogError($"User with id {request.UserId} was not found");
                throw new InvalidOperationException($"User with id {request.UserId} was not found");
            }

            user = _mapper.Map(request, user);

            var identityUserResult = await _userManager.UpdateAsync(user);

            if (!identityUserResult.Succeeded)
            {
                _logger.LogError($"User update failed");
                throw new InvalidOperationException($"User update failed");
            }

            if (!await _userManager.IsInRoleAsync(user, request.SystemRole))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var removeResult = await _userManager.RemoveFromRolesAsync(user, userRoles);
                if (!removeResult.Succeeded)
                {
                    throw new InvalidOperationException($"Can't remove system roles from User with ID '{user.Id}'.");
                }

                var identityRoleResult = await _userManager.AddToRoleAsync(user, request.SystemRole);
                if (!identityRoleResult.Succeeded)
                {
                    throw new InvalidOperationException($"System Role of User with ID '{user.Id}' not updated.");
                }
            }

            return user.Id;
        }
    }
}
