using AutoMapper;
using GerenciamentoDeEndereco.DTO;
using GerenciamentoDeEndereco.Infra;
using GerenciamentoDeEndereco.Model;
using GerenciamentoDeEndereco.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace GerenciamentoDeEndereco.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    [AllowAnonymous]
    public class UsuarioController : ControllerBase
    {
        private readonly UserDbContext _db; // Contexto do banco de dados
        private readonly IMapper _mapper; // Serviço de mapeamento DTO
        private readonly IEmailService _emailService;

        // Construtor para injeção de dependências
        public UsuarioController(UserDbContext db, IMapper mapper, IEmailService emailService)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db)); // Garante que o contexto do banco de dados não seja nulo
            _mapper = mapper;
            _emailService = emailService;

        }

        [NonAction]
        public async Task<Usuario> getCurrentUser()
        {
            // Verifica se o usuário está autenticado
            if (!User.Identity.IsAuthenticated)
            {
                throw new InvalidOperationException("Usuário não autenticado.");
            }

            // Obtém a claim do NameIdentifier, que contém o ID do usuário
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);

            // Verifica se a claim é nula
            if (claim == null)
            {
                throw new InvalidOperationException("Claim 'NameIdentifier' não encontrada.");
            }

            // Converte o valor da claim para long
            if (!long.TryParse(claim.Value, out var id))
            {
                throw new InvalidOperationException("O valor da claim 'NameIdentifier' não é um número válido.");
            }

            // Encontra o usuário no banco de dados com base no ID
            var usuario = await _db.Usuarios.FindAsync(id);

            // Verifica se o usuário foi encontrado
            if (usuario == null)
            {
                throw new InvalidOperationException("Usuário não encontrado.");
            }

            return usuario;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            try
            {
                // Obtém o usuário atual
                var currentUser = await getCurrentUser();

                // Encontra o usuário com o ID fornecido
                var usuario = await _db.Usuarios.FindAsync(currentUser.id);

                // Mapeia o usuário para o modelo de resposta e retorna a resposta OK
                return Ok(_mapper.Map<UsuarioResponse>(usuario));
            }
            catch (Exception ex)
            {
                // Retorna um erro interno se ocorrer uma exceção
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao buscar usuário. Por favor, tente novamente mais tarde.");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(UsuarioDTO DTO)
        {
            try
            {
                // Mapeia o DTO para o modelo de usuário
                var usuario = _mapper.Map<Usuario>(DTO);

                // Adiciona o usuário ao banco de dados e salva as alterações
                var usuarioSalvo = await _db.Usuarios.AddAsync(usuario);
                await _db.SaveChangesAsync();

                // Cria a URI para o novo recurso
                var uri = new Uri($"{Request.Scheme}://{Request.Host}/usuario/{usuarioSalvo.Entity.id}");
                var usuarioResponse = _mapper.Map<UsuarioResponse>(usuarioSalvo.Entity);

                // Retorna uma resposta de criação com a URI do novo recurso
                return Created(uri, usuarioResponse);
            }
            catch (Exception ex)
            {
                // Retorna um erro interno se ocorrer uma exceção
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao criar usuário. Por favor, tente novamente mais tarde.");
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                // Obtém o usuário atual
                var currentUser = await getCurrentUser();

                // Verifica se o ID do usuário atual corresponde ao ID fornecido
                if (currentUser.id != id) return Unauthorized("Permissão insuficiente para realizar essa ação.");

                // Encontra e remove o usuário com o ID fornecido
                var usuario = _db.Usuarios.Find(id);

                if (usuario == null)
                {
                    return NotFound("Usuário não encontrado.");
                }

                _db.Usuarios.Remove(usuario);
                await _db.SaveChangesAsync();

                // Retorna uma resposta sem conteúdo para indicar sucesso
                return NoContent();
            }
            catch (Exception ex)
            {
                // Retorna um erro interno se ocorrer uma exceção
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao deletar usuário. Por favor, verifique suas permissões de usuário.");
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(long id, [FromBody] UsuarioEdicaoDTO dadosNovos)
        {
            try
            {
                // Valida os dados de entrada
                dadosNovos.validateDate();

                // Obtém o usuário atual
                var currentUser = await getCurrentUser();
                if (currentUser.id != id) return Unauthorized("Permissão insuficiente para realizar essa ação.");

                // Encontra o usuário com o ID fornecido
                var usuario = await _db.Usuarios.FindAsync(id);

                // Atualiza os dados do usuário com base no DTO fornecido
                if (dadosNovos.nomeCompleto != null) usuario.nomeCompleto = dadosNovos.nomeCompleto;
                if (dadosNovos.email != null) usuario.email = dadosNovos.email;
                if (dadosNovos.senha != null) usuario.senha = dadosNovos.senha;

                // Atualiza o usuário no banco de dados e salva as alterações
                _db.Usuarios.Update(usuario);
                await _db.SaveChangesAsync();

                // Mapeia o usuário para o modelo de resposta e retorna a resposta OK
                var usuarioResponse = _mapper.Map<UsuarioResponse>(usuario);
                return Ok(usuarioResponse);
            }
            catch (Exception ex)
            {
                // Retorna um erro interno se ocorrer uma exceção
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao tentar atualizar usuário : " + ex.Message);
            }
        }

        [HttpPut("{token}")]
        [Authorize]
        public async Task<IActionResult> redefinirSenha(string token, [FromBody] NovaSenha dados)
        {
            try
            {
                dados.validaDadosEntrada();

                // Valida token de recuperação
                if (!token.IsNullOrEmpty()) return BadRequest("Token não incluso na requisição.");

                var tokenRedefinicaoSenha = await _db.PasswordResetTokens.FindAsync(token);

                if (!token.IsNullOrEmpty()) return BadRequest("Token inválido.");

                string padrao = @"^(?=.*[A-Z])(?=.*[^\w\s]).+$";
                if (!Regex.IsMatch(dados.novaSenha, padrao) || dados.novaSenha.Length < 6) throw new Exception("Senha invalida");

                // Obtém o usuário atual
                Usuario usuario = await getCurrentUser();

                // Atualiza a senha do usuário
                usuario.senha = dados.novaSenha;

                // Atualiza o usuário no banco de dados e salva as alterações
                _db.Usuarios.Update(usuario);
                await _db.SaveChangesAsync();
                var usuarioResponse = _mapper.Map<UsuarioResponse>(usuario);

                // Retorna a resposta OK com os dados do usuário atualizado
                return Ok(usuarioResponse);
            }
            catch (Exception ex)
            {
                // Retorna um erro interno se ocorrer uma exceção
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao tentar atualizar usuário : " + ex.Message);
            }
        }


        // Endpoint para solicitar redefinição de senha
        [HttpPost("esqueceu-a-senha")]
        public async Task<IActionResult> SolicitarRedefinicaoSenha([FromBody] RedefinirSenha redefinirSenha)
        {
            try
            {

                // Validar e-mail
                redefinirSenha.ValidaDadosEntrada();

                // Verifica se existe um usuário com o e-mail fornecido
                var user = await _db.Usuarios.FirstOrDefaultAsync(u => u.email == redefinirSenha.email);

                if (user == null)
                    return BadRequest("Usuário não encontrado.");

                // Gerar um token de redefinição de senha
                var token = TokenGenerator.GenerateToken();

                // Define a data de expiração do token
                var expiration = DateTime.UtcNow.AddHours(1); // Válido por 1 hora

                // Usar injeção de dependência para o serviço de token
                var tokenService = new SaveTokenToDataBase(_db);

                // Salvar token no banco de dados, associado ao usuário
                await tokenService.SaveTokenToDatabaseAsync(redefinirSenha.email, token, expiration);

                // Enviar o e-mail com o link de email
                await _emailService.SendPasswordResetEmailAsync(redefinirSenha.email, token);

                return Ok("Instruções para redefinir a senha foram enviadas por e-mail.");
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao tentar atualizar usuário: " + error.Message);
            }
        }

    }
}
