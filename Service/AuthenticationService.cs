using GerenciamentoDeEndereco.DTO;
using GerenciamentoDeEndereco.Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace GerenciamentoDeEndereco.Service
{
    public class AuthenticationService
    {

        private readonly UserDbContext _db;
        private readonly JwtService _jwtService;

        public AuthenticationService(UserDbContext db, JwtService jwtService)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _jwtService = jwtService;
        }

        public async Task<String> post([FromBody] LoginDTO dto)
        {
                var sqlQuery = "SELECT * FROM Users WHERE email = @Email AND password = @Password";

                var user = await _db.Users.FromSqlRaw(sqlQuery,
                new MySqlParameter("@Email", dto.Email),
                new MySqlParameter("@Password", dto.Password)
                ).FirstOrDefaultAsync();

                if (user != null)
                {
                    var token = _jwtService.GenerateToken(user.Id.ToString());
                    return token;
                }
                
            throw new Exception("Usuário não encontrado");
        }
    }
}
