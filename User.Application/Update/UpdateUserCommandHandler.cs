using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace User.Application.Update
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Guid>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        public UpdateUserCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Guid> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());

            if (user == null)
                throw new InvalidOperationException(nameof(user));

            user.BirthDate = request.BirthDate;
            user.Departament = request.Departament;
            user.JobTitle = request.JobTitle;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;

            var identityUserResult = await _userManager.UpdateAsync(user);

            if (!identityUserResult.Succeeded)
            {
                throw new InvalidOperationException(nameof(identityUserResult));
            }

            return user.Id;
        }
    }
}
