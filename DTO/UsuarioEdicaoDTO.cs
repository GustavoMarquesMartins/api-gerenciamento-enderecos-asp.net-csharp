using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace GerenciamentoDeEndereco.DTO
{
    public class UsuarioEdicaoDTO
    {
        public string? nomeCompleto { get; set; }

        public string? email { get; set; }

        public string? senha { get; set; }

        public void validateDate()
        {
            if (nomeCompleto != null)
            {
                var nomeCompletoEsperado = nomeCompleto + " ";
                string padrao = @"^(([A-Z][a-z]{1,})\s{1}){2,}$";
                if (!Regex.IsMatch(nomeCompletoEsperado, padrao)) throw new ArgumentException("o campo nome completo e inválido.");
            }
           
            if(email != null)
            {
                string padrao = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(email, padrao)) throw new ArgumentException("o campo e-mail e inválido.");

            }

            if (senha != null)
            {
                string padrao = @"^(?=.*[A-Z])(?=.*[^\w\s]).+$";
                if (!Regex.IsMatch(senha, padrao) || senha.Length < 6) throw new SenhaInvalidaException();

            }

        }
    }
    public class SenhaInvalidaException : Exception
    {
        public SenhaInvalidaException() : base("A senha deve atender aos seguintes critérios:\n" +
                                               "- Deve conter um ou mais caracteres especiais.\n" +
                                               "- Deve conter pelo menos uma letra maiúscula.\n" +
                                               "- Deve ter um comprimento mínimo de 6 caracteres.")
        {
        }
    }
}
