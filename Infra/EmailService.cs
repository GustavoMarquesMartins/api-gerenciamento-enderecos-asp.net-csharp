using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace GerenciamentoDeEndereco.Infra
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpEmail; // Variável para armazenar o e-mail SMTP
        private readonly string _smtpPassword; // Variável para armazenar a senha SMTP

        public EmailService(string smtpEmail, string smtpPassword)
        {
            // Acessa as configurações de e-mail e senha do arquivo de configuração
            _smtpEmail = smtpEmail ?? throw new ArgumentException("SMTP email is not configured.");
            _smtpPassword = smtpPassword ?? throw new ArgumentException("SMTP password is not configured.");
        }

        public async Task SendPasswordResetEmailAsync(string email, string tokenRedefinicaoSenha)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));
            if (string.IsNullOrEmpty(tokenRedefinicaoSenha))
                throw new ArgumentException("Token cannot be null or empty.", nameof(tokenRedefinicaoSenha));

            var message = new MimeMessage
            {
                From = { new MailboxAddress("Equipe de Suporte", _smtpEmail) },
                To = { new MailboxAddress("Cliente", email) },
                Subject = "Redefinição de senha",
                Body = new TextPart("plain")
                {
                    Text = $"Acesse o link para modificação de senha: {tokenRedefinicaoSenha}" // Inclua o token no corpo do e-mail
                }
            };

            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_smtpEmail, _smtpPassword);
                await client.SendAsync(message);
            }
            catch (Exception ex)
            {
                // Log o erro de forma apropriada (em vez de apenas escrever no console)
                // Por exemplo: _logger.LogError($"Erro ao enviar e-mail: {ex.Message}");
                Console.WriteLine($"Erro ao enviar e-mail: {ex.Message}");
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }
    }
}
