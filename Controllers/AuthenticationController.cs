using AddressManagement.Service;
using GerenciamentoDeEndereco.DTO;
using GerenciamentoDeEndereco.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AddressManagement.Controllers
{
    /// <summary>
    /// Controller for authentication-related operations.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class Authentication : ControllerBase
    {
        private readonly AuthenticationService _authenticationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="Authentication"/> class with the specified services.
        /// </summary>
        /// <param name="authenticationService">The service responsible for authentication operations.</param>
        public Authentication(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Authenticates a user and generates a JWT token.
        /// </summary>
        /// <param name="dto">The login Data Transfer Object containing user credentials.</param>
        /// <returns>An <see cref="ActionResult"/> containing the generated JWT token.</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LoginDTO dto)
        {
            try
            {
                var token = await _authenticationService.Post(dto);
                return Ok(token);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during authentication: " + error.Message);
            }
        }
    }
}
