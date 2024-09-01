namespace GerenciamentoDeEndereco.Model
{
    public class PasswordResetToken
    {
        public int Id { get; set; } // ID único para o token
        public string Email{ get; set; } // ID do usuário associado ao token
        public string Token { get; set; } // Token gerado
        public DateTime Expiration { get; set; } // Data de expiração do token

        public PasswordResetToken setEmail(string email)
        {
            this.Email = email;
            return this;
        }

        public PasswordResetToken setToken(string token)
        {
            this.Token = token;
            return this;
        }

        public PasswordResetToken setExpiration(DateTime expiration)
        {
            this.Expiration = expiration;
            return this;
        }
    }
}
