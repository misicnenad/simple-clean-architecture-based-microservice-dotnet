using System.Collections.Generic;
using System.Threading.Tasks;

using AccountManager.API.Models;
using AccountManager.Domain.Queries;

using AutoMapper;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountManager.API.Controllers
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
        /// Returns the accounts for the specified userId
        /// </summary>
        /// <response code="200">If the accounts were found</response>
        /// <response code="400">If the userId provided was of an invalid type</response> 
        [HttpGet("{userId}/accounts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<AccountDto>>> GetAccountsByUserIdAsync(int userId)
        {
            var query = new GetAccountsByUserId(userId);

            var accounts = await _mediator.Send(query);

            return Ok(_mapper.Map<IEnumerable<AccountDto>>(accounts));
        }
    }
}