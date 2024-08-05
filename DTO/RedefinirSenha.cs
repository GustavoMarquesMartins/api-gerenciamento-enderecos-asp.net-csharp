using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace GerenciamentoDeEndereco.DTO
{
    public class RedefinirSenha
    {
        [Required]
        public string email { get; set; }

        // Construtor padrão necessário para a deserialização
        public RedefinirSenha() { }

        // Validação do e-mail
        public void ValidaDadosEntrada()
        {
            if (email != null)
            {
                string padrao = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(email, padrao))
                {
                    throw new ArgumentException("O campo e-mail é inválido.");
                }
            }
        }
    }
}
