using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace GerenciamentoDeEndereco.DTO
{
    [AllowAnonymous]
    public class UserDTO
    {
        [Required(ErrorMessage = "O campo nome completo não pode ficar em branco")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo nome e-mail não pode ficar em branco")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo senha não pode ficar em branco")]
        [MinLength(6, ErrorMessage ="A senha precisa ter um número mínimo de 6 caracteres")]
        public string Password { get; set; }

    }
}
