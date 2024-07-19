using AutoMapper;
using GerenciamentoDeEndereco.DTO;
using GerenciamentoDeEndereco.Infra;
using GerenciamentoDeEndereco.Model;
using GerenciamentoDeEndereco.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GerenciamentoDeEndereco.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    [AllowAnonymous]
    public class UsuarioController : ControllerBase
    {
        private UserDbContext _db;
        private IMapper _mapper;

        public UsuarioController(UserDbContext db, IMapper mapper)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mapper = mapper;
        }

        [NonAction]
        public Usuario getCurrentUser()
        {
            var id = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var usuario = _db.Usuarios.Find(id);
            return usuario != null ? usuario : throw new InvalidOperationException("Usuário não encontrado");
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(long id)
        {
            try
            {
                var currentUserId = getCurrentUser().id;

                if (currentUserId != id) return Unauthorized("Você não tem permissão suficiente para visualizar os dados solicitados");

                var usuario = await _db.Usuarios.FindAsync(id);

                return Ok(_mapper.Map<UsuarioResponse>(usuario));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao buscar usuário. Por favor, tente novamente mais tarde.");
            }
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(UsuarioDTO DTO)
        {
            try
            {
                var usuario = _mapper.Map<Usuario>(DTO);

                var usuarioSalvo = await _db.Usuarios.AddAsync(usuario);
                await _db.SaveChangesAsync();

                var uri = new Uri($"{Request.Scheme}://{Request.Host}/usuario/{usuarioSalvo.Entity.id}");
                var usuarioResponse = _mapper.Map<UsuarioResponse>(usuarioSalvo.Entity);

                return Created(uri, usuarioResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao criar usuário. Por favor, tente novamente mais tarde.");
            }
        }


        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var currentUserId = getCurrentUser().id;

                if (currentUserId != id) return Unauthorized("Permissão insuficiente para realizar essa ação.");

                var usuario = _db.Usuarios.Find(id);

                _db.Usuarios.Remove(usuario);

                await _db.SaveChangesAsync();
                
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao deletar usuário. Por favor, verifique suas permissões de usuário.");
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(long id, [FromBody] UsuarioEdicaoDTO dadosNovos)
        {
            try
            {
                dadosNovos.validateDate();

                var currentUserId = getCurrentUser().id;
                if (currentUserId != id) return Unauthorized("Permissão insuficiente para realizar essa ação.");

                var usuario = await _db.Usuarios.FindAsync(id);

                if (dadosNovos.nomeCompleto != null) usuario.nomeCompleto = dadosNovos.nomeCompleto;
                if (dadosNovos.nomeUsuario != null) usuario.nomeUsuario = dadosNovos.nomeUsuario;
                if (dadosNovos.senha != null) usuario.senha = dadosNovos.senha;

                _db.Usuarios.Update(usuario);
                await _db.SaveChangesAsync();

                var usuarioResponse = _mapper.Map<UsuarioResponse>(usuario);
                return Ok(usuarioResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao tentar atualizar usuário : " + ex.Message);
            }
        }
    }
}
