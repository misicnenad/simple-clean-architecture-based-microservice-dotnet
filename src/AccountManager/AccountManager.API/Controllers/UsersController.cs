using System.Collections.Generic;
using System.Threading.Tasks;
using AccountManager.API.Models;
using AccountManager.Domain.Queries;
using AutoMapper;
using MediatR;
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

        [HttpGet("{userId}/accounts")]
        public async Task<ActionResult<IEnumerable<AccountDto>>> GetAccountsByUserIdAsync(int userId)
        {
            var query = new GetAccountsByUserId(userId);

            var accounts = await _mediator.Send(query);

            return Ok(_mapper.Map<IEnumerable<AccountDto>>(accounts));
        }
    }
}