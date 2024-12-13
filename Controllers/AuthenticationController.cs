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
        /// Constructor that initializes the Authentication controller with dependencies.
        /// </summary>
        /// <param name="authenticationService">Service for authentication operations</param>
        public Authentication(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Authenticates a user and generates a JWT token.
        /// </summary>
        /// <param name="dto">Login Data Transfer Object containing user credentials</param>
        /// <returns>ActionResult containing the generated JWT token</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LoginDTO dto)
        {
            try
            {
                var token = await _authenticationService.Post(dto);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An internal error occurred. Details: " + ex.Message);
            }
        }
    }
}
