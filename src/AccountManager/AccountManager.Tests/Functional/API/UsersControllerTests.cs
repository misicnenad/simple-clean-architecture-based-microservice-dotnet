using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using AccountManager.API.Models;
using AccountManager.Domain.Models;
using AccountManager.Infrastructure.Models;

using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;

using Xunit;

namespace AccountManager.Tests.Functional.API
{
    public class UsersControllerTests : TestFixture
    {
        private const string _apiUrl = "api/users";

        [Fact]
        public async Task GetAccountsByUserIdAsync_Returns_Accounts()
        {
            // Arrange
            var host = await hostBuilder.StartAsync();
            var client = host.GetTestClient();

            var userId = 1;
            var expectedAccounts = new List<AccountDbo>
            {
                new AccountDbo
                {
                    AccountId = 1,
                    UserId = userId,
                    AccountType = AccountType.Credit,
                    DateCreated = DateTime.UtcNow,
                },
                new AccountDbo
                {
                    AccountId = 2,
                    UserId = userId,
                    AccountType = AccountType.Debit,
                    DateCreated = DateTime.UtcNow,
                }
            };
            using var context = host.Resolve<AccountManagerDbContext>();
            context.Accounts.AddRange(expectedAccounts);
            context.SaveChanges();

            // Act
            var response = await client.GetAsync($"{_apiUrl}/{userId}/accounts");

            // Assert
            Assert.True(response.IsSuccessStatusCode);

            var accounts = await response.Content.ReadAsAsync<IEnumerable<AccountDto>>();
            Assert.NotNull(accounts);
            Assert.Equal(expectedAccounts.Count, accounts.Count());

            Assert.Equal(expectedAccounts[0].AccountId, accounts.ElementAt(0).AccountId);
            Assert.Equal(expectedAccounts[0].UserId, accounts.ElementAt(0).UserId);
            Assert.Equal(expectedAccounts[0].AccountType, accounts.ElementAt(0).AccountType);
            Assert.Equal(expectedAccounts[0].DateCreated, accounts.ElementAt(0).DateCreated);

            Assert.Equal(expectedAccounts[1].AccountId, accounts.ElementAt(1).AccountId);
            Assert.Equal(expectedAccounts[1].UserId, accounts.ElementAt(1).UserId);
            Assert.Equal(expectedAccounts[1].AccountType, accounts.ElementAt(1).AccountType);
            Assert.Equal(expectedAccounts[1].DateCreated, accounts.ElementAt(1).DateCreated);
        }

        [Fact]
        public async Task GetAccountsByUserIdAsync_Returns_Empty_Collection()
        {
            // Arrange
            var host = await hostBuilder.StartAsync();
            var client = host.GetTestClient();

            var userId = 1;

            // Act
            var response = await client.GetAsync($"{_apiUrl}/{userId}/accounts");

            // Assert
            Assert.True(response.IsSuccessStatusCode);

            var accounts = await response.Content.ReadAsAsync<IEnumerable<AccountDto>>();
            Assert.NotNull(accounts);
            Assert.Empty(accounts);
        }
    }
}
