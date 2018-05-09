using Bizfitech.Banking.Api.Core.Dtos;
using Bizfitech.Banking.Api.Core.Interfaces;
using Bizfitech.Banking.Api.Web.Controllers;
using Bizfitech.Banking.Api.Web.Helpers;
using Bizfitech.Banking.Api.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bizfitech.Banking.Api.Web.Tests.Controllers
{
    [TestClass]
    public class UsersControllerTests
    {
        [ClassInitialize()]
        public static void Initialise(TestContext context)
        {
            Mappers.Initialise();
        }

        [TestMethod]
        public async Task Post_WithInvalidModel_ShouldReturnBadRequest()
        {
            //Arrange
            var usersController = new UsersController(null, null);
            var modelStateDictionary = new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary();
            usersController.ModelState.AddModelError("", "error");

            //Act
            var result = await usersController.Post(null);

            //Assert
            Assert.IsTrue(result is BadRequestObjectResult);
        }

        [TestMethod]
        public async Task Post_WithValidModel_ShouldReturnCreatedStatus()
        {
            //Arrange
            var modelStateDictionary = new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary();
            
            var userPostDto = new UserPostDto
            {
                BankAccount = new BankAccountDto { AccountNumber = "12345678", BankName = "ABank" },
                Email = "Email@Email.com",
                FirstName = "Jason",
                LastName = "Wilson",
                Username = "Email@Email.com"
            };

            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(m => m.AddAsync(It.IsAny<UserCreateDto>()))
                .ReturnsAsync(
                        new Core.Models.Result<Core.Models.User>
                        {
                            ErrorCategory = Core.Models.ErrorCategory.NoError,
                            Obj = new Core.Models.User { }
                        }
                );

            var usersController = new UsersController(mockUserService.Object, null);

            //Act
            var result = await usersController.Post(userPostDto);

            //Assert
            mockUserService.Verify(m => m.AddAsync(It.IsAny<UserCreateDto>()), Times.Once);
            Assert.IsTrue(result is CreatedResult);
            var createdResult = result as CreatedResult;
        }

        [TestMethod]
        public async Task Post_WhenBadData_ShouldReturn400Status()
        {
            //Arrange
            var modelStateDictionary = new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary();
            
            var userPostDto = new UserPostDto
            {
                BankAccount = new BankAccountDto { AccountNumber = "12345678", BankName = "ABank" },
                Email = "Email@Email.com",
                FirstName = "Jason",
                LastName = "Wilson",
                Username = "Email@Email.com"
            };

            const string badDataMessage = "bad data";

            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(m => m.AddAsync(It.IsAny<UserCreateDto>()))
                .ReturnsAsync(
                        new Core.Models.Result<Core.Models.User>
                        {
                            ErrorCategory = Core.Models.ErrorCategory.BadData,
                            Message = badDataMessage,
                            Obj = new Core.Models.User { }
                        }
                );

            var usersController = new UsersController(mockUserService.Object, null);

            //Act
            var result = await usersController.Post(userPostDto);

            //Assert
            mockUserService.Verify(m => m.AddAsync(It.IsAny<UserCreateDto>()), Times.Once);
            Assert.IsTrue(result is BadRequestObjectResult);
        }

        [TestMethod]
        public async Task Post_WhenServiceReturnsInternalServerError_ShouldReturn500Status()
        {
            //Arrange
            var modelStateDictionary = new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary();
            
            var userPostDto = new UserPostDto
            {
                BankAccount = new BankAccountDto { AccountNumber = "12345678", BankName = "ABank" },
                Email = "Email@Email.com",
                FirstName = "Jason",
                LastName = "Wilson",
                Username = "Email@Email.com"
            };

            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(m => m.AddAsync(It.IsAny<UserCreateDto>()))
                .ReturnsAsync(
                        new Core.Models.Result<Core.Models.User>
                        {
                            ErrorCategory = Core.Models.ErrorCategory.InternalServerError,
                            Obj = new Core.Models.User { }
                        }
                );

            var usersController = new UsersController(mockUserService.Object, null);

            //Act
            var result = await usersController.Post(userPostDto);

            //Assert
            mockUserService.Verify(m => m.AddAsync(It.IsAny<UserCreateDto>()), Times.Once);
            var statusCodeResult = result as StatusCodeResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(500, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public async Task Post_WhenServiceReturnsNotFound_ShouldReturn404Status()
        {
            //Arrange
            var modelStateDictionary = new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary();
            
            var userPostDto = new UserPostDto
            {
                BankAccount = new BankAccountDto { AccountNumber = "12345678", BankName = "ABank" },
                Email = "Email@Email.com",
                FirstName = "Jason",
                LastName = "Wilson",
                Username = "Email@Email.com"
            };

            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(m => m.AddAsync(It.IsAny<UserCreateDto>()))
                .ReturnsAsync(
                        new Core.Models.Result<Core.Models.User>
                        {
                            ErrorCategory = Core.Models.ErrorCategory.NotFound,
                            Obj = new Core.Models.User { }
                        }
                );

            var usersController = new UsersController(mockUserService.Object, null);

            //Act
            var result = await usersController.Post(userPostDto);

            //Assert
            mockUserService.Verify(m => m.AddAsync(It.IsAny<UserCreateDto>()), Times.Once);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }
    }
}
