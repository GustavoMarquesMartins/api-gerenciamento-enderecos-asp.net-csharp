using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace GerenciamentoDeEndereco.DTO
{
    public class ResetPasswordDTO
    {
        [Required]
        public string Token{ get; set; }  

        [Required]
        public string NewPassword { get; set; }

        [Required]
        public string NewPasswordConfirm { get; set; }

        public void validaDadosEntrada()
        {
            if (NewPassword != null)
            {
                string padrao = @"^(?=.*[A-Z])(?=.*[^\w\s]).+$";
                if (!Regex.IsMatch(NewPassword, padrao) || NewPassword.Length < 6) throw new PasswordInvalidaException();
            }

            if(NewPasswordConfirm != NewPassword) throw new PasswordInvalidaException();
        }
    }
}
