using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AccountService.Controllers;
using AccountService.Models;
using AccountService.Repository;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ControllerTests
{
    public class AccountControllerTests
    {
        [Fact]
        public void GivenAListOfAccount_WhenGetAccountsCalled_ShouldReturnOkObjectResult()
        {
            // Given
            var mockRepository = new Mock<IAccountRepository>();
            mockRepository.Setup(repo => repo.GetAccounts()).Returns(GetAccounts());
            var accountController = new AccountController(mockRepository.Object);

            // When
            var result = accountController.Get();

            // Then
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GivenAListOfAccount_WhenGetAccountsCalled_ShouldReturnCorrectAccounts()
        {
            // Given
            var mockRepository = new Mock<IAccountRepository>();
            mockRepository.Setup(repo => repo.GetAccounts()).Returns(GetAccounts());
            var accountController = new AccountController(mockRepository.Object);

            // When
            var result = accountController.Get();

            // Then
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var accounts = Assert.IsAssignableFrom<IEnumerable<Account>>(viewResult.Value);
            Assert.Equal(2, accounts.Count());
        }

        [Fact]
        public void GivenARepositoryThrowsException_WhenGetAccountsCalled_ShouldReturnInternalServerError()
        {
            // Given
            var mockRepository = new Mock<IAccountRepository>();
            mockRepository.Setup(repo => repo.GetAccounts())
                .Throws(new Exception("Database exception"));
            var accountController = new AccountController(mockRepository.Object);

            // When
            var result = accountController.Get();

            // Then
            var viewResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal((int) HttpStatusCode.InternalServerError, viewResult.StatusCode);
        }

        [Fact]
        public void GivenListOfAccount_WhenGetAccountByIdCalled_ShouldReturnCorrectAccount()
        {
            // Given
            var mockRepository = new Mock<IAccountRepository>();
            mockRepository.Setup(repo => repo.GetAccountById(1))
                .Returns(GetAccounts().Single(account => account.Id == 1));
            var accountController = new AccountController(mockRepository.Object);

            // When
            var result = accountController.Get(1);

            // Then
            var viewResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(viewResult.Value);
        }

        [Fact]
        public void GivenAccountDoesNotExistsWithProvidedId_WhenGetAccountByIdCalled_ShouldReturnNotFoundObjectResult()
        {
            // Given
            var mockRepository = new Mock<IAccountRepository>();
            var accountController = new AccountController(mockRepository.Object);

            // When
            var result = accountController.Get(2);

            // Then
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void GivenAccountDoesNotExistsWithProvidedId_WhenGetAccountByIdCalled_ShouldReturnExpectedMessage()
        {
            // Given
            var mockRepository = new Mock<IAccountRepository>();
            var accountController = new AccountController(mockRepository.Object);

            // When
            var result = accountController.Get(2);

            // Then
            var viewResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Account with ID '2' doesn't exists.", viewResult.Value);
        }

        [Fact]
        public void GivenARepositoryThrowsException_WhenGetAccountByIdCalled_ShouldReturnInternalServerError()
        {
            // Given
            var mockRepository = new Mock<IAccountRepository>();
            mockRepository.Setup(repo => repo.GetAccountById(1))
                .Throws(new Exception("Database exception"));
            var accountController = new AccountController(mockRepository.Object);

            // When
            var result = accountController.Get(1);

            // Then
            var viewResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, viewResult.StatusCode);
        }

        [Fact]
        public void GivenADummyAccount_WhenPostAccountCalled_ShouldReturnCreatedAtActionResult()
        {
            // Given
            var dummyAccount = new Account();
            var mockRepository = new Mock<IAccountRepository>();
            var accountController = new AccountController(mockRepository.Object);

            // When
            var result = accountController.Post(dummyAccount);

            // Then
            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public void GivenAnAccount_WhenPostAccountCalled_ShouldReturnConflictObjectResult()
        {
            // Given
            var account = new Account();
            var mockRepository = new Mock<IAccountRepository>();
            mockRepository.Setup(repository => repository.GetAccountByName(It.IsAny<string>())).Returns(account);
            var accountController = new AccountController(mockRepository.Object);

            // When
            var result = accountController.Post(account);

            // Then
            Assert.IsType<ConflictObjectResult>(result);
        }

        private static IEnumerable<Account> GetAccounts()
        {
            var accounts = new List<Account>
            {
                new Account()
                {
                    Email = "test",
                    FirstName = "abc",
                    LastName = "test",
                    Id = 1,
                    Name = "test",
                    Password = "test"
                },
                new Account()
                {
                    Email = "test",
                    FirstName = "abc",
                    LastName = "test",
                    Id = 2,
                    Name = "test2",
                    Password = "test"
                }
            };

            return accounts;
        }
    }
}
