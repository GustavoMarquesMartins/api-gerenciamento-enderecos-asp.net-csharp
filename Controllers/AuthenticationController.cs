using GerenciamentoDeEndereco.DTO;
using GerenciamentoDeEndereco.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace GerenciamentoDeEndereco.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Authentication : ControllerBase
    {
        private readonly AuthenticationService _authenticationService;

        public Authentication(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<IActionResult> post([FromBody] LoginDTO dto)
        {
            try
            {
                var token = await _authenticationService.post(dto);
                return Ok(token);
            }
            catch (Exception ex)
            { 
                return StatusCode(500, "Ocorreu um erro interno. Detalhes: " + ex.Message);
            }
        }
    }
}
