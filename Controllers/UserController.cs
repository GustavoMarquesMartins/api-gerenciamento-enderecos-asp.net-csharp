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
        private readonly EmailService _emailService;

        public UserController(UserService usuarioService, CommonService commonService, EmailService emailService)
        {
            _userService = usuarioService ?? throw new ArgumentNullException(nameof(usuarioService));
            _commonService = commonService ?? throw new ArgumentNullException(nameof(CommonService));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok((await _userService.get()));
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao buscar usuário. Por favor, tente novamente mais tarde.");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] UserDTO dto)
        {
            try
            {
                var userResponse = await _userService.post(dto);
                Uri uri = await _commonService.getUri(this, "");

                return Created(uri, userResponse);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao cadastrar usuário. Por favor, tente novamente mais tarde.");
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete()
        {
            try
            {
                await _userService.delete();
                return NoContent();
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao deletar usuário. Por favor, verifique suas permissões de usuário.");
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Put([FromBody] UserUpdateDTO dto)
        {
            try
            {
               var userResponse = await _userService.put(dto);
                return Ok(userResponse);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao tentar atualizar usuário : " + error.Message);
            }
        }

        [HttpPost("esqueceu-a-senha")]
        public async Task<IActionResult> requestPasswordReset([FromBody] ForgotPasswordDTO dto)
        {
            try
            {
                var token = await _emailService.requestPasswordReset(dto);
                return Ok(token);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao tentar solicitar alteração de senha: " + error.Message);
            }
        }

        [HttpPost("valida-token-redefinicao")]
        public async Task<IActionResult> resetPassword([FromBody] ResetPasswordDTO dto)
        {
            try
            {
               var userResponse = await _emailService.resetPassword(dto);
                return Ok(userResponse);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao tentar atualizar senha : " + error.Message);
            }
        }
    }
}
