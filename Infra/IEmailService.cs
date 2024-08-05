namespace GerenciamentoDeEndereco.Infra
{
    public interface IEmailService
    {
        Task SendPasswordResetEmailAsync(string email, string tokenRedefinicaoSenha);
    }
}
