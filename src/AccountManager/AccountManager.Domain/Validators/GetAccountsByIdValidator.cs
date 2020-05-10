using System.Collections.Generic;
using AccountManager.Domain.Models;
using AccountManager.Domain.Queries;

namespace AccountManager.Domain.Validators
{
    public class GetAccountsByIdValidator : QueryValidator<GetAccountsByUserId, IEnumerable<Account>>
    {
    }
}
