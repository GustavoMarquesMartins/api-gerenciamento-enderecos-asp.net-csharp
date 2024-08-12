using System.ComponentModel.DataAnnotations;

namespace GerenciamentoDeEndereco.DTO
{
    public class LoginDTO
    {
        [Required]
        public string email { get; set; }
        [Required]
        public string senha { get; set; }

    }
}
