using GerenciamentoDeEndereco.Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace GerenciamentoDeEndereco.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Autenticacao : ControllerBase
    {
        private readonly UserDbContext _db;

        public Autenticacao(UserDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string usuario, string senha)
        {
            try
            {
                var sqlQuery = "SELECT * FROM Usuarios WHERE nomeUsuario = @Usuario AND senha = @Senha";

                // Executa a consulta SQL passando os parâmetros corretamente
                var usuarios = await _db.Usuarios.FromSqlRaw(sqlQuery,
                    new MySqlParameter("@Usuario", usuario),
                    new MySqlParameter("@Senha", senha)
                ).ToListAsync();

                return (usuarios != null && usuarios.Count > 0) ? Ok("Usuário existente") : NotFound("Usuário não encontrado");

            }catch (Exception ex)
            { 
                return StatusCode(500, "Ocorreu um erro interno. Detalhes: " + ex.Message);
            }
        }
    }
}
