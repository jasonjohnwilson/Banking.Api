using Bizfitech.Banking.Api.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace Bizfitech.Banking.Api.Core.Tests.Serevices
{
    [TestClass]
    public class BankApiClientProviderServicesTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Get_WithNullBankParam_ShouldThrowArgumentNullException()
        {
            //arrange
            var clientsDictionary = new Dictionary<string, BankApiClientBase>();
            var provider = new BankApiClientProvider(clientsDictionary);

            //act
            provider.Get(null);

            //assert
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Get_WhenClientForBankNotFound_ShouldThrowArgumentException()
        {
            //arrange
            var clientsDictionary = new Dictionary<string, BankApiClientBase>();
            var provider = new BankApiClientProvider(clientsDictionary);

            //act
            provider.Get("bank");

            //assert
        }

        [TestMethod]
        public void Get_WhenClientForBankFound_ShouldReturnClient()
        {
            //arrange
            var bank = "bank";
            var mockClient = new Mock<BankApiClientBase>(null);
            var clientsDictionary 
                = new Dictionary<string, BankApiClientBase>() { { bank, mockClient.Object } };
            var provider = new BankApiClientProvider(clientsDictionary);

            //act
            var client = provider.Get(bank);

            //assert
            Assert.IsNotNull(client);
        }
    }
}
