using AutoMapper;
using GerenciamentoDeEndereco.DTO;
using GerenciamentoDeEndereco.Infra;
using GerenciamentoDeEndereco.Model;
using GerenciamentoDeEndereco.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mysqlx.Crud;
using System.Security.Claims;

namespace GerenciamentoDeEndereco.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EnderecoController : ControllerBase
    {
        // Declaração de variáveis de contexto do banco de dados e do AutoMapper.
        private UserDbContext _db;
        private readonly IMapper _mapper;

        // Construtor que inicializa o contexto do banco de dados e o AutoMapper.
        public EnderecoController(UserDbContext db, IMapper mapper)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // Método para obter o usuário atual logado a partir dos claims do token.
        [NonAction]
        public Usuario getCurrentUser()
        {
            var id = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var usuario = _db.Usuarios.Find(id);
            if (usuario == null) NotFound("Usuário não é válido");
            return usuario;
        }

        // Endpoint para obter todos os endereços do usuário atual.
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var listaEnderecos = await _db.Enderecos
                    .Where(e => e.usuarioId == getCurrentUser().id)
                    .ToListAsync();

                if (listaEnderecos == null || listaEnderecos.Count == 0)
                {
                    return NotFound("Nenhum endereço encontrado.");
                }

                var listaEnderecosResponse = listaEnderecos
                    .Select(endereco => _mapper.Map<EnderecoResponse>(endereco))
                    .ToList();

                return Ok(listaEnderecosResponse);
            }
            catch (Exception ex)
            {
                // Log do erro
                Console.WriteLine($"Erro ao obter lista de endereços: {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao buscar endereços. Por favor, tente novamente mais tarde.");
            }
        }

        // Endpoint para obter um endereço específico pelo ID.
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            try
            {
                var endereco = await _db.Enderecos.FindAsync(id);

                if (endereco == null || endereco.usuarioId != getCurrentUser().id) return NotFound("Endereço não encontrado!");

                var enderecoResponse = _mapper.Map<EnderecoResponse>(endereco);

                return Ok(enderecoResponse);
            }
            catch (Exception ex)
            {
                // Log do erro
                Console.WriteLine($"Erro ao buscar endereço com ID {id}: {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao buscar endereço. Por favor, tente novamente mais tarde.");
            }
        }

        // Endpoint para adicionar um novo endereço.
        [HttpPost]
        public async Task<IActionResult> Post(EnderecoDTO DTO)
        {
            try
            {
                var endereco = _mapper.Map<Endereco>(DTO);

                endereco.usuarioId = getCurrentUser().id;

                var enderecoSalvo = await _db.Enderecos.AddAsync(endereco);
                await _db.SaveChangesAsync();

                var uri = new Uri($"{Request.Scheme}://{Request.Host}/enderecos/{enderecoSalvo.Entity.id}");

                var enderecoResponse = _mapper.Map<EnderecoResponse>(enderecoSalvo.Entity);

                return Created(uri, enderecoResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao salvar o endereço: {ex.Message}");
            }
        }

        // Endpoint para deletar um endereço pelo ID.
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var endereco = await _db.Enderecos.FindAsync(id);

            if (endereco == null || endereco.usuarioId != getCurrentUser().id) return NotFound("Endereço não encontrado!");

            _db.Enderecos.Remove(endereco);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        // Endpoint para atualizar dados de um endereço pelo ID.
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarDadosEndereco(long id, [FromBody] EnderecoEdicaoDTO novosDados)
        {
            var endereco = await _db.Enderecos.FindAsync(id);

            if (endereco == null || endereco.usuarioId != getCurrentUser().id) return NotFound("Endereço não encontrado!");

            if (novosDados.cep != null) endereco.cep = novosDados.cep;
            if (novosDados.logradouro != null) endereco.logradouro = novosDados.logradouro;
            if (novosDados.complemento != null) endereco.complemento = novosDados.complemento;
            if (novosDados.uf != null) endereco.uf = novosDados.uf;
            if (novosDados.numero != null) endereco.numero = novosDados.numero;

            _db.Enderecos.Update(endereco);
            await _db.SaveChangesAsync();

            var enderecoResponse = _mapper.Map<EnderecoResponse>(endereco);

            return Ok(enderecoResponse);
        }
    }
}
