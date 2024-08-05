namespace GerenciamentoDeEndereco.Model
{
    public class PasswordResetToken
    {
        public int Id { get; set; } // ID único para o token
        public string UserId { get; set; } // ID do usuário associado ao token
        public string Token { get; set; } // Token gerado
        public DateTime Expiration { get; set; } // Data de expiração do token
        
    }
}
