using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.Identity;
using Swashbuckle.AspNetCore.Annotations;
using User.Application.GetAllUsers;
using User.Application.GetUserById;
using User.Application.Update;
using User.Application.Users.Disable;

namespace User.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {

        private readonly ILogger<UsersController> _logger;
        private readonly IMediator _mediator;


        public UsersController(ILogger<UsersController> logger, 
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [SwaggerOperation("Get all Users.")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success.", typeof(IEnumerable<ApplicationUser>))]
        public async Task<IActionResult> Get()
        {
            var users = await _mediator.Send(new GetAllUsersQuery());

            return Ok(users);
        }

        [HttpGet]
        [Route("{userId:guid}")]
        [SwaggerOperation("Get User by ID.")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success.", typeof(ApplicationUser))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "User was not found.")]
        public async Task<IActionResult> GetByUserId([FromRoute] Guid userId)
        {
            var user = await _mediator.Send(new GetUserByIdQuery
            {
                UserId = userId
            });

            if (user == null)
            {
                return NotFound($"User with ID '{userId}' was not found.");
            }

            return Ok(user);
        }

        [HttpPut]
        [Route("{userId:guid}")]
        [SwaggerOperation("Endpoint to User self update.", "Authorized User has access.")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success.", typeof(Guid))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid User model.")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Internal server error.")]
        public async Task<IActionResult> Update([FromRoute] Guid userId, [FromBody] UpdateUserCommand command)
        {
            command.UserId = userId;
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPost]
        [Route("{userId:guid}/disable")]
        [SwaggerOperation("Endpoint to Disable user by Admin", "Authorized User has access.")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success.", typeof(Guid))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Internal server error.")]
        public async Task<IActionResult> Disable([FromRoute] Guid userId)
        {
            var result = await _mediator.Send(new DisableUserCommand { UserId = userId });

            return Ok(result);
        }
    }
}
