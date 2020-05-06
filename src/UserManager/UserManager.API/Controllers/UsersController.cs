using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace UserManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IEnumerable<int> _existingUserIds = new List<int> { 1, 3, 5, 7, 9 };

        [HttpGet("{userId}")]
        public ActionResult<bool> GetDoesUserExist(int userId)
        {
            return _existingUserIds.Contains(userId);
        }
    }
}