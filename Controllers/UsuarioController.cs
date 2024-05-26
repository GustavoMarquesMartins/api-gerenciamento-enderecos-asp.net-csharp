using GerenciamentoDeEndereco.DTO;
using GerenciamentoDeEndereco.Infra;
using GerenciamentoDeEndereco.Model;
using GerenciamentoDeEndereco.Response;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoDeEndereco.Controllers
{
    [ApiController]
    [Route("/usuarios")]
    public class UsuarioController : ControllerBase
    {
        private UserDbContext _db;

        public UsuarioController(UserDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        [HttpGet]
        public IActionResult Get()
        {
            var listaUsuarios= _db.Usuarios.ToList();
            var listaUsuariosResponse = converteListaDeUsuariosEmObjetosDeResposta(listaUsuarios);
            return Ok(listaUsuariosResponse);
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var usuario = _db.Usuarios.Find(id);

            if (usuario == null)
            {
                return NotFound("Endereço não encontrado!");
            }

            return Ok(converteUsuarioEmObjetoDeResposta(usuario));
        }

        [HttpPost]
        public IActionResult Post(UsuarioDTO DTO)
        {
            validarDadosUsuario(DTO);
            var usuario = defineDadosUsuario(DTO);

            var usuarioSalvo = _db.Usuarios.Add(usuario);
            _db.SaveChanges();

            var idUsuario = usuarioSalvo.Entity.id;
            var uri = new Uri($"{Request.Scheme}://{Request.Host}/usuarios/{idUsuario}");

            return Created(uri, converteUsuarioEmObjetoDeResposta(usuarioSalvo.Entity));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var usuario = _db.Usuarios.Find(id);

            if (usuario == null)
            {
                return NotFound("Endereço não encontrado!");
            }

            _db.Usuarios.Remove(usuario);
            _db.SaveChanges();
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult AtualizarDadosUsuario(long id, [FromBody] UsuarioDTO usuarioAtualizado)
        {
            validarDadosUsuario(usuarioAtualizado);
            var usuarioDesatualizado = _db.Usuarios.Find(id);

            if (usuarioDesatualizado == null)
            {
                return NotFound("Usuário não encontrado!");
            }

            usuarioDesatualizado.nomeCompleto = usuarioAtualizado.nomeCompleto;
            usuarioDesatualizado.nomeUsuario = usuarioAtualizado.nomeUsuario;
            usuarioDesatualizado.senha = usuarioAtualizado.senha;


            var usuario = _db.Usuarios.Update(usuarioDesatualizado);
            _db.SaveChanges();

            return Ok(converteUsuarioEmObjetoDeResposta(usuario.Entity));
        }

        private Usuario defineDadosUsuario(UsuarioDTO DTO)
        {
            var usuario = new Usuario();

            usuario.nomeCompleto = DTO.nomeCompleto;
            usuario.nomeUsuario = DTO.nomeUsuario;
            usuario.senha = DTO.senha;

            return usuario;
        }

        private List<UsuarioResponse> converteListaDeUsuariosEmObjetosDeResposta(List<Usuario> listaUsuario)
        {
            List<UsuarioResponse> listaResponse = new List<UsuarioResponse>();

            foreach (Usuario usuario in listaUsuario)
            {
                var usuarioResponse = new UsuarioResponse();

                usuarioResponse.nomeUsuario = usuario.nomeUsuario;
                usuarioResponse.nomeCompleto = usuario.nomeCompleto;
                usuarioResponse.senha = usuario.senha;
                
                listaResponse.Add(usuarioResponse);
            }
            return listaResponse;
        }

        private UsuarioResponse converteUsuarioEmObjetoDeResposta(Usuario usuario)
        {
            var usuarioResponse = new UsuarioResponse();

            usuarioResponse.nomeCompleto = usuario.nomeCompleto;
            usuarioResponse.nomeUsuario = usuario.nomeUsuario;
            usuarioResponse.senha = usuario.senha;

            return usuarioResponse;
        }

        private void validarDadosUsuario(UsuarioDTO DTO)
        {
         
        }
    }
}
