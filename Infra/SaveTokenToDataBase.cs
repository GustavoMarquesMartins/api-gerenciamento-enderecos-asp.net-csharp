using GerenciamentoDeEndereco.Model;

namespace GerenciamentoDeEndereco.Infra
{
    public class SaveTokenToDataBase
    {
        readonly UserDbContext _db;

        public SaveTokenToDataBase(UserDbContext db)
        {
            _db = db;
        }


        public async Task SaveTokenToDatabaseAsync(string userId, string token, DateTime expiration)
        {
          
                // Cria uma nova entidade de token
                var passwordResetToken = new PasswordResetToken
                {
                    UserId = userId,
                    Token = token,
                    Expiration = expiration,
                };

                // Adiciona o token ao DbSet
                _db.PasswordResetTokens.Add(passwordResetToken);

                // Salva as alterações no banco de dados
                await _db.SaveChangesAsync();
            }

    }
}

