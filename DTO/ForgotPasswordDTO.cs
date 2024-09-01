using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace GerenciamentoDeEndereco.DTO
{
    public class ForgotPasswordDTO
    {
        [Required]
        public string Email { get; set; }

        // Validação do e-mail
        public void ValidaDadosEntrada()
        {
            if (Email != null)
            {
                string padrao = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(Email, padrao))
                {
                    throw new ArgumentException("O campo e-mail não corresponde ao padrão esperado.");
                }
            }
        }
    }
}
