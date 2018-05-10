using Bizfitech.Banking.Api.Core.Dtos;
using Bizfitech.Banking.Api.Core.Interfaces;
using Bizfitech.Banking.Api.Core.Models;
using Bizfitech.Banking.Api.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bizfitech.Banking.Api.Core.Tests.Serevices
{
    [TestClass]
    public class UserServiceTests
    {
        [TestMethod]
        public async Task AddAsync_WithNullDto_ShouldReturnInternalError()
        {
            //arrange
            var userService = new UserService(null, null);

            //act
            var result = await userService.AddAsync(null);

            //assert
            Assert.IsTrue(
                result.ErrorCategory == Models.ErrorCategory.InternalServerError &&
                result.Message == "Value cannot be null.\r\nParameter name: user cannot be null"
                );
        }

        [TestMethod]
        public async Task AddAsync_WhenUsersBankNotFoud_ShouldReturnNotFound()
        {
            //arrange
            var mockBankRepository = new Mock<IGenericRepository<Bank>>();
            mockBankRepository.Setup(m => m.GetAllAsync(It.IsAny<Expression<Func<Bank, bool>>>()))
                .ReturnsAsync(Enumerable.Empty<Bank>().AsQueryable());

            var mockUow = new Mock<IBankUow>();
            mockUow.SetupGet(p => p.Banks)
                .Returns(() => mockBankRepository.Object);
            var userService = new UserService(mockUow.Object, null);
            var userDto = new UserCreateDto { BankAccount = new BankAccountCreateDto { BankName = "Bank" } };

            //act
            var result = await userService.AddAsync(userDto);

            //assert
            Assert.IsTrue(
                result.ErrorCategory == Models.ErrorCategory.NotFound);
        }

        [TestMethod]
        public async Task AddAsync_AccountIsInvalid_ShouldReturnNotFound()
        {
            //arrange
            var bank = "bank";
            var accountNumber = "12345678";

            var mockAccountService = new Mock<IAccountService>();
            mockAccountService.Setup(m => m.IsAccountValidAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            var mockBankRepository = new Mock<IGenericRepository<Bank>>();
            mockBankRepository.Setup(m => m.GetAllAsync(It.IsAny<Expression<Func<Bank, bool>>>()))
                .ReturnsAsync((new Bank[] { new Bank { } }).AsQueryable());

            var mockUow = new Mock<IBankUow>();
            mockUow.SetupGet(p => p.Banks)
                .Returns(() => mockBankRepository.Object);

            var userService = new UserService(mockUow.Object, mockAccountService.Object);
            var userDto = new UserCreateDto { BankAccount = new BankAccountCreateDto { BankName = bank,  AccountNumber = accountNumber} };

            //act
            var result = await userService.AddAsync(userDto);

            //assert
            Assert.IsTrue(
                result.ErrorCategory == Models.ErrorCategory.NotFound);
            mockAccountService.Verify(m => m.IsAccountValidAsync(It.Is<string>(v => v == bank), It.Is<string>(v => v == accountNumber)), Times.Once);
        }

        [TestMethod]
        public async Task AddAsync_WhenAccountNotAssignedToAnotherUser_ShouldSaveData()
        {
            //arrange
            var bank = "bank";
            var accountNumber = "12345678";

            var mockAccountService = new Mock<IAccountService>();
            mockAccountService.Setup(m => m.IsAccountValidAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var mockUserRepository = new Mock<IGenericRepository<User>>();
            mockUserRepository.Setup(m => m.AddAsync(It.IsAny<User>()))
                .Returns(Task.CompletedTask);

            var mockAccountRepository = new Mock<IGenericRepository<Account>>();
            mockAccountRepository.Setup(m => m.GetAllAsync(It.IsAny<Expression<Func<Account, bool>>>()))
                .ReturnsAsync(Enumerable.Empty<Account>().AsQueryable());
            mockAccountRepository.Setup(m => m.AddAsync(It.IsAny<Account>()))
                .Returns(Task.CompletedTask);

            var mockBankRepository = new Mock<IGenericRepository<Bank>>();
            mockBankRepository.Setup(m => m.GetAllAsync(It.IsAny<Expression<Func<Bank, bool>>>()))
                .ReturnsAsync((new Bank[] { new Bank { } }).AsQueryable());

            var mockUow = new Mock<IBankUow>();
            mockUow.SetupGet(p => p.Banks)
                .Returns(() => mockBankRepository.Object);
            mockUow.SetupGet(p => p.Accounts)
                .Returns(() => mockAccountRepository.Object);
            mockUow.SetupGet(p => p.Users)
                .Returns(() => mockUserRepository.Object);
            mockUow.Setup(m => m.SaveAllChangesAsync()).ReturnsAsync(1);

            var userService = new UserService(mockUow.Object, mockAccountService.Object);
            var userDto = new UserCreateDto {Username = "jwilson", BankAccount = new BankAccountCreateDto { BankName = bank, AccountNumber = accountNumber } };

            //act
            var result = await userService.AddAsync(userDto);

            //assert
            Assert.IsTrue(
                result.ErrorCategory == Models.ErrorCategory.NoError);
            mockUow.Verify(m => m.SaveAllChangesAsync(), Times.Once);
            mockUserRepository.Verify(m => m.AddAsync(It.IsAny<User>()), Times.Once);
            mockAccountRepository.Verify(m => m.AddAsync(It.IsAny<Account>()), Times.Once);
        }
    }
}
