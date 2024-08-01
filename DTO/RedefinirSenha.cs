using System.ComponentModel.DataAnnotations;

namespace GerenciamentoDeEndereco.DTO
{
    public class RedefinirSenha
    {
        [Required]
        public string senha{ get; set; }
    }
}
