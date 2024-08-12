namespace GerenciamentoDeEndereco.Model
{
    public class PasswordResetToken
    {
        public int id { get; set; } // ID único para o token
        public string email { get; set; } // ID do usuário associado ao token
        public string token { get; set; } // Token gerado
        public DateTime expiration { get; set; } // Data de expiração do token

        public PasswordResetToken setEmail(string email)
        {
            this.email = email;
            return this;
        }

        public PasswordResetToken setToken(string token)
        {
            this.token = token;
            return this;
        }

        public PasswordResetToken setExpiration(DateTime expiration)
        {
            this.expiration = expiration;
            return this;
        }
    }
}
