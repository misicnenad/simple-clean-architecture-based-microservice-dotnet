using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManager.API.Models;
using UserManager.Domain.Models;
using UserManager.Domain.Queries;
using UserManager.Infrastructure.Configurations;

namespace UserManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public UsersController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns all users
        /// </summary>
        /// <response code="200">If the users were found</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsersAsync()
        {
            var query = new RequestWrapper<GetAllUsers, IEnumerable<User>>(new GetAllUsers());

            var users = await _mediator.Send(query);

            return Ok(_mapper.Map<IEnumerable<UserDto>>(users));
        }
    }
}