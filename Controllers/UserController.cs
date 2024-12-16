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
    /// Provides API endpoints for user-related operations.
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
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="userService">Service for handling user operations.</param>
        /// <param name="commonService">Service providing common functionalities.</param>
        /// <param name="passwordResetService">Service for handling password reset operations.</param>
        /// <exception cref="ArgumentNullException">Thrown when a dependency is null.</exception>
        public UserController(UserService userService, CommonService commonService, PasswordResetService passwordResetService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _commonService = commonService ?? throw new ArgumentNullException(nameof(commonService));
            _passwordResetService = passwordResetService ?? throw new ArgumentNullException(nameof(passwordResetService));
        }

        /// <summary>
        /// Retrieves the authenticated user details.
        /// </summary>
        /// <returns>An <see cref="ActionResult"/> containing the user details.</returns>
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
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal error retrieving user. Please try again later. Error details: {error.Message}");
            }
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="dto">The data transfer object for user creation.</param>
        /// <returns>An <see cref="ActionResult"/> containing the created user details.</returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] UserDTO dto)
        {
            try
            {
                var userResponse = await _userService.Post(dto);
                Uri uri = _commonService.GetUri(this, userResponse.Id.ToString());

                return Created(uri, userResponse);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal error creating user. Please try again later. Error details: {error.Message}");
            }
        }

        /// <summary>
        /// Deletes the authenticated user.
        /// </summary>
        /// <returns>No content on successful deletion.</returns>
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
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal error deleting user. Please try again later. Error details: {error.Message}");
            }
        }

        /// <summary>
        /// Updates the authenticated user details.
        /// </summary>
        /// <param name="dto">The data transfer object for updating user details.</param>
        /// <returns>An <see cref="ActionResult"/> containing the updated user details.</returns>
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
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal error updating user. Please try again later. Error details: {error.Message}");
            }
        }

        /// <summary>
        /// Initiates the process to send a password reset email.
        /// </summary>
        /// <param name="dto">The data transfer object containing the email address.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] PasswordResetRequestDTO dto)
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
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal error when sending password reset email. Please try again later. Error details: {error.Message}");
            }
        }

        /// <summary>
        /// Validates the password reset code.
        /// </summary>
        /// <param name="requestCode">The data transfer object containing the reset code.</param>
        /// <returns>An <see cref="IActionResult"/> containing the validation token.</returns>
        [HttpPost("validate-code")]
        public async Task<IActionResult> ValidatePasswordRequestCode([FromBody] PasswordRequestCodeDTO requestCode)
        {
            try
            {
                ValidateInputDataAuthentication.Code(requestCode.Code);
                var token = await _passwordResetService.VerifyCodeValidityAsync(requestCode.Code);
                return Ok(token);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal error validating password reset code. Please try again later. Error details: {error.Message}");
            }
        }

        /// <summary>
        /// Resets the user's password using the provided token and new password.
        /// </summary>
        /// <param name="dto">The data transfer object containing the token, new password, and confirmation of the new password.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
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
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal error updating password. Please try again later. Error details: {error.Message}");
            }
        }
    }
}
