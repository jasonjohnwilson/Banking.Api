using Bizfitech.Banking.Api.Core.Interfaces;
using Bizfitech.Banking.Api.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace Bizfitech.Banking.Api.Web.Tests.Controllers
{
    [TestClass]
    public class AccountServiceTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task IsAccountValidAsync_WhenBankNameIsNull_ShouldThrowException()
        {
            //Arrange
            var accounrService = new AccountService(null);

            //Act
            await accounrService.IsAccountValidAsync(null, "1234567");

            //Assert
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task IsAccountValidAsync_WhenAccountNumberIsNull_ShouldThrowException()
        {
            //Arrange
            var accounrService = new AccountService(null);

            //Act
            await accounrService.IsAccountValidAsync("bankname", null);

            //Assert
        }

        [TestMethod]
        public async Task IsAccountValidAsync_WhenBankAccountNotFound_ShouldReturnFalse()
        {
            //Arrange
            var bank = "bank";
            var account = "12345678";

            var mockClient = new Mock<BankApiClientBase>(null);
            mockClient.Setup(m => m.GetAccountAsync(It.Is<string>(v => v == account)))
                .ReturnsAsync(() => null);

            var mockClientApiProvider = new Mock<IBankApiClientProvider>();
            mockClientApiProvider.Setup(m => m.Get(It.IsAny<string>()))
                .Returns(() => mockClient.Object);

            var accounrService = new AccountService(mockClientApiProvider.Object);

            //Act
            var isValid = await accounrService.IsAccountValidAsync(bank, account);

            //Assert
            Assert.IsFalse(isValid);
            mockClientApiProvider.Verify(m => m.Get(It.Is<string>(v => v == bank)), Times.Once);
        }

        [TestMethod]
        public async Task IsAccountValidAsync_WhenBankAccountFound_ShouldReturnTrue()
        {
            //Arrange
            var bank = "bank";
            var account = "12345678";

            var mockClient = new Mock<BankApiClientBase>(null);
            mockClient.Setup(m => m.GetAccountAsync(It.Is<string>(v => v == account)))
                .ReturnsAsync(new Core.Dtos.BankAccountReadDto());

            var mockClientApiProvider = new Mock<IBankApiClientProvider>();
            mockClientApiProvider.Setup(m => m.Get(It.IsAny<string>()))
                .Returns(() =>  mockClient.Object);

            var accounrService = new AccountService(mockClientApiProvider.Object);

            //Act
            var isValid = await accounrService.IsAccountValidAsync(bank, account);

            //Assert
            Assert.IsTrue(isValid);
            mockClientApiProvider.Verify(m => m.Get(It.Is<string>(v => v == bank)), Times.Once);
        }
    }
}
