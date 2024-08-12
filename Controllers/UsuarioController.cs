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

namespace GerenciamentoDeEndereco.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    [AllowAnonymous]
    public class UsuarioController : ControllerBase
    {
        private readonly UserDbContext _db; // Contexto do banco de dados
        private readonly IMapper _mapper; // Serviço de mapeamento DTO
        private readonly EmailService _emailService; // Serviço de envio de e-mail

        // Construtor para injeção de dependências
        public UsuarioController(UserDbContext db, IMapper mapper, EmailService emailService)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db)); // Garante que o contexto do banco de dados não seja nulo
            _mapper = mapper; // Inicializa o serviço de mapeamento
            _emailService = emailService; // Inicializa o serviço de e-mail
        }

        // Método para obter o usuário autenticado atual
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

        // Método para verificar a validade do token de redefinição de senha
        [NonAction]
        private async Task<PasswordResetToken> verificaValidadeToken(string token)
        {
            // Valida a presença do token
            if (token.IsNullOrEmpty()) throw new ArgumentNullException("Token não pode ser um campo em branco.");

            // Verifica se o token consta no banco de dados
            var relacionamentoUsuarioToken = _db.PasswordResetTokens.FirstOrDefault(u => u.token == token);
            if (relacionamentoUsuarioToken == null) throw new Exception("Token inválido");

            // Verifica a validade do token
            if (relacionamentoUsuarioToken.expiration < DateTime.Now) throw new Exception("Token expirado. Favor fazer a solicitação novamente!");

            return relacionamentoUsuarioToken;
        }

        // Endpoint para obter os dados do usuário atual
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

        // Endpoint para criar um novo usuário
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

        // Endpoint para deletar um usuário
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

        // Endpoint para atualizar os dados de um usuário
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

        // Endpoint para validar o token de redefinição de senha e redefinir a senha do usuário
        [HttpPost("valida-token-redefinicao")]
        public async Task<IActionResult> redefinirSenha([FromBody] NovaSenha dados)
        {
            try
            {
                // Valida os dados de entrada
                dados.validaDadosEntrada();

                // Valida se o token é válido
                var relacionamento = await verificaValidadeToken(dados.token);

                // Obtém o usuário associado ao token
                Usuario usuario = await _db.Usuarios.FirstOrDefaultAsync(u => u.email == relacionamento.email);

                // Verifica se a senha antiga fornecida é correta e se a nova senha é diferente da antiga
                if (usuario.senha != dados.senhaAntiga) throw new Exception("senha antiga incorreta!");
                if (usuario.senha == dados.novaSenha) throw new Exception("A nova senha tem que ser diferente da senha antiga");

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
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao tentar atualizar senha : " + ex.Message);
            }
        }

        // Endpoint para solicitar redefinição de senha
        [HttpPost("esqueceu-a-senha")]
        public async Task<IActionResult> SolicitarRedefinicaoSenha([FromBody] RedefinirSenha redefinirSenha)
        {
            try
            {
                // Valida o e-mail fornecido
                redefinirSenha.ValidaDadosEntrada();

                // Verifica se existe um usuário com o e-mail fornecido
                var user = await _db.Usuarios.FirstOrDefaultAsync(u => u.email == redefinirSenha.email);

                if (user == null)
                    return NotFound("Usuário não encontrado.");

                // Gera um token de redefinição de senha
                var token = TokenGenerator.GenerateToken();

                // Define a data de expiração do token
                var expiration = DateTime.UtcNow.AddHours(1); // Válido por 1 hora

                var tokenService = new PasswordResetToken()
                    .setToken(token)
                    .setEmail(redefinirSenha.email)
                    .setExpiration(expiration);

                // Adiciona o token de redefinição ao banco de dados
                await _db.PasswordResetTokens.AddAsync(tokenService);
                await _db.SaveChangesAsync();

                // Envia o e-mail com o link para redefinir a senha
                await _emailService.SendPasswordResetEmailAsync(redefinirSenha.email, token);

                // Retorna uma resposta de sucesso
                return Ok("Instruções para redefinir a senha foram enviadas por e-mail.");
            }
            catch (Exception error)
            {
                // Retorna um erro interno se ocorrer uma exceção
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao tentar solicitar alteração de senha: " + error.Message);
            }
        }

    }
}
