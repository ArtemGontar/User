using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using MediatR;
using Shared.Identity;
using User.Application.GetAllUsers;
using User.Application.GetUserById;

namespace User.Controllers
{
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
        //[SwaggerOperation("Get all Users.")]
        //[SwaggerResponse((int)HttpStatusCode.OK, "Success.", typeof(IEnumerable<ApplicationUser>))]
        public async Task<IActionResult> Get()
        {
            var users = await _mediator.Send(new GetAllUsersQuery());

            return Ok(users);
        }

        [HttpGet]
        [Route("{userId:guid}")]
        //[SwaggerOperation("Get User by ID.")]
        //[SwaggerResponse((int)HttpStatusCode.OK, "Success.", typeof(ApplicationUser))]
        //[SwaggerResponse((int)HttpStatusCode.NotFound, "User was not found.")]
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
    }
}
