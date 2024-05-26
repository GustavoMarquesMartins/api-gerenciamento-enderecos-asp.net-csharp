using AutoMapper;
using GerenciamentoDeEndereco.DTO;
using GerenciamentoDeEndereco.Infra;
using GerenciamentoDeEndereco.Model;
using GerenciamentoDeEndereco.Response;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoDeEndereco.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EnderecoController : ControllerBase
    {
        private UserDbContext _db;
        private readonly IMapper _mapper;

        public EnderecoController(UserDbContext db, IMapper mapper)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public IActionResult Get()
        {
            var listaEnderecos = _db.Enderecos.ToList();

            var listaEnderecosResponse = listaEnderecos
                .Select(endereco => _mapper.Map<EnderecoResponse>(endereco))
                .ToList();

            return Ok(listaEnderecosResponse);
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var endereco = _db.Enderecos.Find(id);

            if (endereco == null)
            {
                return NotFound("Endereço não encontrado!");
            }

            var enderecoResponse = _mapper.Map<EnderecoResponse>(endereco);

            return Ok(enderecoResponse);
        }

        [HttpPost]
        public IActionResult Post(EnderecoDTO DTO)
        {
            var usuarioSolicitado = _db.Usuarios.Find(DTO.usuarioId);
            if (usuarioSolicitado == null)
            {
                return NotFound("Usuário solicitado não encontrado!");
            }

            var endereco = _mapper.Map<Endereco>(DTO);
            var enderecoSalvo = _db.Enderecos.Add(endereco);
            _db.SaveChanges();

            var uri = new Uri($"{Request.Scheme}://{Request.Host}/enderecos/{enderecoSalvo.Entity.id}");
            var enderecoResponse = _mapper.Map<EnderecoResponse>(enderecoSalvo.Entity);

            return Created(uri, enderecoResponse);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var endereco = _db.Enderecos.Find(id);

            if (endereco == null)
            {
                return NotFound("Endereço não encontrado!");
            }

            _db.Enderecos.Remove(endereco);
            _db.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult AtualizarDadosEndereco(long id, [FromBody] EnderecoDTO novosDados)
        {
            var endereco = _db.Enderecos.Find(id);

            if (endereco == null)
            {
                return NotFound("Endereço não encontrado!");
            }

            if (novosDados.cep != null)
            {
                endereco.cep = novosDados.cep;
            }

            if (novosDados.logradouro != null)
            {
                endereco.logradouro = novosDados.logradouro;
            }

            if (novosDados.complemento != null)
            {
                endereco.complemento = novosDados.complemento;
            }

            if (novosDados.uf != null)
            {
                endereco.uf = novosDados.uf;
            }

            if (novosDados.numero != null)
            {
                endereco.numero = novosDados.numero;
            }

            var enderecoAtualizado = _db.Enderecos.Update(endereco);
            _db.SaveChanges();

            var enderecoResponse = _mapper.Map<EnderecoResponse>(enderecoAtualizado.Entity);

            return Ok(enderecoResponse);
        }
    }
}
