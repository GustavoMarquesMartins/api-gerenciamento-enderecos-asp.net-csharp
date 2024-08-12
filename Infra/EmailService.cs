using Azure.Core;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GerenciamentoDeEndereco.Infra
{
    public class EmailService
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
            string htmlTemplate = await LerConteudoHtmlAsync("./assets/HTML/PasswordResetEmail.html");
            string urlRedefinicaoSenha = "http://127.0.0.1:5501/redefinir-senha.html?token=" + HttpUtility.UrlEncode(tokenRedefinicaoSenha);

            var htmlMessage = htmlTemplate
                .Replace("{{ResetLink}}", urlRedefinicaoSenha)
                .Replace("{{UserName}}", "Nome do Usuário"); 


            var message = new MimeMessage
            {
                From = { new MailboxAddress("Equipe de Suporte", _smtpEmail) },
                To = { new MailboxAddress("Cliente", email) },
                Subject = "Redefinição de senha",
                Body = new TextPart("html")
                {
                    Text = htmlMessage
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
                Console.WriteLine($"Erro ao enviar e-mail: {ex.Message}");
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }
        public async Task<string> LerConteudoHtmlAsync(string caminhoDoArquivo)
        {
            // Abre o arquivo para leitura
            using (var fileStream = new FileStream(caminhoDoArquivo, FileMode.Open, FileAccess.Read))
            using (var reader = new StreamReader(fileStream))
            {
                // Lê o conteúdo do arquivo e retorna como uma string
                return await reader.ReadToEndAsync();
            }
        }
    }
}
