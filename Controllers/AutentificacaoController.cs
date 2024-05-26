using GerenciamentoDeEndereco.DTO;
using GerenciamentoDeEndereco.Infra;
using GerenciamentoDeEndereco.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace GerenciamentoDeEndereco.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous] 
    public class Autenticacao : ControllerBase
    {
        private readonly UserDbContext _db;
        private readonly JwtService _jwtService;

        public Autenticacao(UserDbContext db, JwtService jwtService)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _jwtService = jwtService;

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] LoginDTO login)
        {
            try
            {
                var sqlQuery = "SELECT * FROM Usuarios WHERE nomeUsuario = @Usuario AND senha = @Senha";

                var user = await _db.Usuarios.FromSqlRaw(sqlQuery,
                new MySqlParameter("@Usuario", login.usuario),
                new MySqlParameter("@Senha", login.senha)
           ).FirstOrDefaultAsync();

                if (user != null)
                {
                    var token = _jwtService.GenerateToken(user.id.ToString());
                    return Ok(new { token });
                }
                else
                {
                    return NotFound("Usuário não encontrado");
                }
            }
            catch (Exception ex)
            { 
                return StatusCode(500, "Ocorreu um erro interno. Detalhes: " + ex.Message);
            }
        }
    }
}
