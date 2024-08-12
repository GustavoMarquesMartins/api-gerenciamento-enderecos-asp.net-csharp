using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace GerenciamentoDeEndereco.DTO
{
    public class NovaSenha
    {
        [Required]
        public string token{ get; set; }  

        [Required]
        public string senhaAntiga { get; set; }

        [Required]
        public string novaSenha { get; set; }

        [Required]
        public string confirmacaoSenha { get; set; }

        public void validaDadosEntrada()
        {
            if (senhaAntiga == null) throw new SenhaInvalidaException();

            if (novaSenha != null)
            {
                string padrao = @"^(?=.*[A-Z])(?=.*[^\w\s]).+$";
                if (!Regex.IsMatch(novaSenha, padrao) || novaSenha.Length < 6) throw new SenhaInvalidaException();
            }

            if(confirmacaoSenha != novaSenha) throw new SenhaInvalidaException();
        }
    }
}
