using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using UserManager.API.Models;
using UserManager.Domain.Models;
using UserManager.Infrastructure.Models;

using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;

using Xunit;

namespace UserManager.Tests.Functional.API
{
    public class UsersControllerTests : TestFixture
    {
        private const string _apiUrl = "api/users";

        [Fact]
        public async Task GetAllUsers_Returns_Empty_Collection()
        {
            // Arrange
            var host = await hostBuilder.StartAsync();
            var client = host.GetTestClient();

            // Act
            var response = await client.GetAsync(_apiUrl);

            // Assert
            Assert.True(response.IsSuccessStatusCode);

            var users = await response.Content.ReadAsAsync<IEnumerable<UserDto>>();
            Assert.NotNull(users);
            Assert.Empty(users);
        }
    }
}
