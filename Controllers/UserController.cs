using AutoMapper;
using GerenciamentoDeEndereco.DTO;
using GerenciamentoDeEndereco.Infra;
using GerenciamentoDeEndereco.Model;
using GerenciamentoDeEndereco.Response;
using GerenciamentoDeEndereco.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GerenciamentoDeEndereco.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    [AllowAnonymous]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly CommonService _commonService;

        /// <summary>
        /// Constructor that initializes the UserController with dependencies.
        /// </summary>
        /// <param name="userService">Service for user operations</param>
        /// <param name="commonService">Common service for shared functionalities</param>
        /// <param name="emailService">Service for email operations</param>
        /// <exception cref="ArgumentNullException">Thrown when a dependency is null</exception>
        public UserController(UserService userService, CommonService commonService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _commonService = commonService ?? throw new ArgumentNullException(nameof(CommonService));
        }

        /// <summary>
        /// Retrieves the authenticated user details.
        /// </summary>
        /// <returns>ActionResult containing the user details</returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _userService.Get());
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal error retrieving user. Please try again later.");
            }
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="dto">Data Transfer Object for user creation</param>
        /// <returns>ActionResult containing the created user details</returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] UserDTO dto)
        {
            try
            {
                var userResponse = await _userService.Post(dto);
                Uri uri = await _commonService.GetUri(this, "");

                return Created(uri, userResponse);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal error creating user. Please try again later.");
            }
        }

        /// <summary>
        /// Deletes the authenticated user.
        /// </summary>
        /// <returns>No content</returns>
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete()
        {
            try
            {
                await _userService.Delete();
                return NoContent();
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal error deleting user. Please check your user permissions.");
            }
        }

        /// <summary>
        /// Updates the authenticated user details.
        /// </summary>
        /// <param name="dto">Data Transfer Object for updating user details</param>
        /// <returns>ActionResult containing the updated user details</returns>
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Put([FromBody] UserUpdateDTO dto)
        {
            try
            {
                var userResponse = await _userService.Put(dto);
                return Ok(userResponse);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal error updating user: " + error.Message);
            }
        }
    }
}
