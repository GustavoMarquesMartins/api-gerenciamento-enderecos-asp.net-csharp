using System.Reflection.Metadata.Ecma335;
using AddressManagement.DTO;
using AddressManagement.Service;
using GerenciamentoDeEndereco.DTO;
using GerenciamentoDeEndereco.Service;
using GerenciamentoDeEndereco.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AddressManagement.Controllers
{
    /// <summary>
    /// Controller for user-related operations.
    /// </summary>
    [ApiController]
    [Route("/[controller]")]
    [AllowAnonymous]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly CommonService _commonService;
        private readonly PasswordResetService _passwordResetService;

        /// <summary>
        /// Constructor that initializes the UserController with dependencies.
        /// </summary>
        /// <param name="userService">Service for user operations</param>
        /// <param name="commonService">Common service for shared functionalities</param>
        /// <param name="passwordResetService">Service for password reset operations</param>
        /// <exception cref="ArgumentNullException">Thrown when a dependency is null</exception>
        public UserController(UserService userService, CommonService commonService, PasswordResetService passwordResetService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _commonService = commonService ?? throw new ArgumentNullException(nameof(commonService));
            _passwordResetService = passwordResetService ?? throw new ArgumentNullException(nameof(passwordResetService));
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
                Uri uri = _commonService.GetUri(this, "");

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

        /// <summary>
        /// Initiates the process to send a password reset email.
        /// </summary>
        /// <param name="dto">The data transfer object containing the email address.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPost("forgot-password")]
        public async Task<IActionResult> Get([FromBody] PasswordResetRequestDTO dto)
        {
            try
            {
                // Initiate the email dispatch process with the provided email address
                await _passwordResetService.InitializeEmailDispatch(dto.Email);
                return Ok();
            }
            catch (Exception error)
            {
                // Return a 500 Internal Server Error response with the error message
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal error when sending email: " + error.Message);
            }
        }

        /// <summary>
        /// Validates the password reset code.
        /// </summary>
        /// <param name="requestCode">The data transfer object containing the reset code.</param>
        /// <returns>An IActionResult containing the validation token.</returns>
        [HttpPost("validate-code")]
        public async Task<IActionResult> ValidatePasswordRequestCode([FromBody] PasswordRequestCodeDTO requestCode)
        {
            var code = requestCode.Code;
            ValidateInputDataAuthentication.Code(code);
            var token = await _passwordResetService.VerifyCodeValidityAsync(code);
            return Ok(token);
        }

        /// <summary>
        /// Resets the user's password using the provided token and new password.
        /// </summary>
        /// <param name="dto">The data transfer object containing the token, new password, and confirmation of the new password.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordChangeRequestDTO dto)
        {
            try
            {
                // Validate the input data contained in the DTO
                dto.ValidateData();
                // Update the user's password with the provided token and new password
                await _passwordResetService.UpdateUserPasswordAsync(dto.Token, dto.NewPassword, dto.NewPasswordConfirm);
                return Ok("Password updated successfully.");
            }
            catch (Exception error)
            {
                // Return a 500 Internal Server Error response with the error message
                return StatusCode(StatusCodes.Status500InternalServerError, "Error when trying to update password: " + error.Message);
            }
        }
    }
}
