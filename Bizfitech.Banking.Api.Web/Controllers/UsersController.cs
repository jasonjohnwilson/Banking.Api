using AutoMapper;
using Bizfitech.Banking.Api.Core.Dtos;
using Bizfitech.Banking.Api.Core.Interfaces;
using Bizfitech.Banking.Api.Core.Models;
using Bizfitech.Banking.Api.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bizfitech.Banking.Api.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IGenericRepository<User> _userRepository;

        public UsersController(
            IUserService userService,
            IGenericRepository<User> userRepository)
        {
            _userService = userService;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Create a new user along with their bank account
        /// </summary>
        /// <param name="user"></param>
        /// <returns>A newly created user</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="404">If any items are not found</response>
        /// <response code="400">If any of the required data is incorrect or missing</response> 
        [HttpPost]
        [ProducesResponseType(typeof(UserGetDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Post([FromBody] UserPostDto user)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            var userCreateeDto = Mapper.Map<UserPostDto, UserCreateDto>(user);

            var result = await _userService.AddAsync(userCreateeDto);
            
            if(result.ErrorCategory != ErrorCategory.NoError)
            {
                return FormatErrorResult(result.ErrorCategory, result.Message);    
            }
            
            var userGetDto = Mapper.Map<User, UserGetDto>(result.Obj);

            return Created($"api/Users/{userGetDto.Id}", userGetDto);
        }

        /// <summary>
        /// Get a user by their unique Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Returns a user</returns>
        /// <response code="200">Returns the user</response>
        /// <response code="404">No user found matching the user ID</response>
        [HttpGet("{userId:guid}")]
        [ProducesResponseType(typeof(IEnumerable<UserGetDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(Guid userId)
        {
            var userModel = (await _userRepository.GetAllAsync(u => u.Id == userId)).SingleOrDefault();

            if(userModel == null)
            {
                return NotFound($"No user found with ID {userId}");
            }

            var userGetDto = Mapper.Map<User, UserGetDto>(userModel);

            return Ok(userGetDto);
        }

        /// <summary>
        /// Returns all users in the system 
        /// </summary>
        /// <returns>A list of users</returns>
        /// <response code="200">Returns a list of users</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserGetDto>), 200)]
        public async Task<IActionResult> GetAll()
        {
            //ToDo: probably best to replace this with a paging method.

            var users = await _userRepository.GetAllAsync(u => true);
            var userDtos = Mapper.Map<IEnumerable<User>, IEnumerable<UserGetDto>>(users);
            
            return Ok(userDtos);
        }

        #region Private Methods

        private IActionResult FormatErrorResult(ErrorCategory errorCategory, string message)
        {
            IActionResult result = null;

            switch (errorCategory)
            {
                case ErrorCategory.BadData:
                    result = BadRequest(message);
                    break;

                case ErrorCategory.NotFound:
                    result = NotFound(message);
                    break;

                case ErrorCategory.InternalServerError:
                    result = StatusCode(StatusCodes.Status500InternalServerError);
                    break;

                default:
                    throw new NotSupportedException($"Error category {errorCategory} is not supported.");

            }

            return result;
        }

        #endregion Private Methods
    }
}
