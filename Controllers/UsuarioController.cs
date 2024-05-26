using AutoMapper;
using GerenciamentoDeEndereco.DTO;
using GerenciamentoDeEndereco.Infra;
using GerenciamentoDeEndereco.Model;
using GerenciamentoDeEndereco.Response;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoDeEndereco.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private UserDbContext _db;
        private IMapper _mapper;

        public UsuarioController(UserDbContext db, IMapper mapper)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var usuario = _db.Usuarios.Find(id);

            if (usuario == null) return NotFound("Endereço não encontrado!");

            return Ok(_mapper.Map<UsuarioResponse>(usuario));
        }

        [HttpPost]
        public IActionResult Post(UsuarioDTO DTO)
        {
            var usuario = _mapper.Map<Usuario>(DTO);
            var usuarioSalvo = _db.Usuarios.Add(usuario);
            _db.SaveChanges();

            var uri = new Uri($"{Request.Scheme}://{Request.Host}/usuarios/{usuarioSalvo.Entity.id}");
            var usuarioResponse = _mapper.Map<UsuarioResponse>(usuarioSalvo.Entity);

            return Created(uri, usuarioResponse);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var usuario = _db.Usuarios.Find(id);

            if (usuario == null) return NotFound("Endereço não encontrado!");

            _db.Usuarios.Remove(usuario);
            _db.SaveChanges();
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult AtualizarDadosUsuario(long id, [FromBody] UsuarioDTO dadosNovos)
        {
            var usuario = _db.Usuarios.Find(id);

            if (usuario == null) return NotFound("Usuário não encontrado!");

            if(dadosNovos.nomeCompleto != null) usuario.nomeCompleto = dadosNovos.nomeCompleto;
            
            if (dadosNovos.nomeUsuario != null) usuario.nomeUsuario = dadosNovos.nomeUsuario;

            if (dadosNovos.nomeUsuario != null) usuario.senha = dadosNovos.senha;

            var usuarioAtualizado= _db.Usuarios.Update(usuario);
            _db.SaveChanges();
            
            var usuarioResponse = _mapper.Map<UsuarioResponse>(usuarioAtualizado.Entity);

            return Ok(usuarioResponse);
        }
    }
}
