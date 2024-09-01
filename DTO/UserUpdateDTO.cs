using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace GerenciamentoDeEndereco.DTO
{
    public class UserUpdateDTO
    {
        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public void validateDate()
        {
            if (Name != null)
            {
                var NameEsperado = Name + " ";
                string padrao = @"^(([A-Z][a-z]{1,})\s{1}){2,}$";
                if (!Regex.IsMatch(NameEsperado, padrao)) throw new ArgumentException("o campo nome completo e inválido.");
            }
           
            if(Email != null)
            {
                string padrao = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(Email, padrao)) throw new ArgumentException("o campo e-mail e inválido.");

            }

            if (Password != null)
            {
                string padrao = @"^(?=.*[A-Z])(?=.*[^\w\s]).+$";
                if (!Regex.IsMatch(Password, padrao) || Password.Length < 6) throw new PasswordInvalidaException();

            }

        }
    }
    public class PasswordInvalidaException : Exception
    {
        public PasswordInvalidaException() : base("A Password deve atender aos seguintes critérios:\n" +
                                               "- Deve conter um ou mais caracteres especiais.\n" +
                                               "- Deve conter pelo menos uma letra maiúscula.\n" +
                                               "- Deve ter um comprimento mínimo de 6 caracteres.")
        {
        }
    }
}
