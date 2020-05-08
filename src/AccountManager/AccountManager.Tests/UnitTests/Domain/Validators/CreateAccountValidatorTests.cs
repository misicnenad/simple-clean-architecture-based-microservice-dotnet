using System;
using System.Collections.Generic;
using System.Text;
using AccountManager.Domain.Commands;
using AccountManager.Domain.Models;
using AccountManager.Domain.Validators;
using FluentValidation;
using Xunit;

namespace AccountManager.Tests.UnitTests.Domain.Validators
{
    public class CreateAccountValidatorTests
    {
        private readonly CreateAccountValidator _validations = new CreateAccountValidator();

        [Fact]
        public void Succeeds()
        {
            // Arrange
            var instance = new CreateAccount(userId: 1, AccountType.Credit);

            // Act
            var exception = Record.Exception(() => _validations.ValidateAndThrow(instance));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void Null_Instance_Throws()
        {
            // Arrange
            CreateAccount instance = null;

            // Act
            var exception = Record.Exception(() => _validations.ValidateAndThrow(instance));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Theory]
        [MemberData(nameof(InvalidInstances))]
        public void Invalid_Instance_Throws(CreateAccount invalidInstance)
        {
            // Act
            var exception = Record.Exception(() => _validations.ValidateAndThrow(invalidInstance));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ValidationException>(exception);
        }

        public static IEnumerable<object[]> InvalidInstances
        {
            get
            {
                yield return new CreateAccount[]
                {
                    new CreateAccount(userId: default, accountType: default)
                };
                yield return new CreateAccount[]
                {
                    new CreateAccount(userId: 1, accountType: default)
                };
                yield return new CreateAccount[]
                {
                    new CreateAccount(userId: default, AccountType.Credit)
                };
            }
        }
    }
}
