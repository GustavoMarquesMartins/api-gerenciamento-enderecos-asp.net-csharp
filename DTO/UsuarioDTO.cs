using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace GerenciamentoDeEndereco.DTO
{
    [AllowAnonymous]
    public class UsuarioDTO
    {
        [Required(ErrorMessage = "O campo nome completo não pode ficar em branco")]
        public string nomeCompleto { get; set; }

        [Required(ErrorMessage = "O campo nome usuário não pode ficar em branco")]
        public string nomeUsuario { get; set; }

        [Required(ErrorMessage = "O campo senha não pode ficar em branco")]
        [MinLength(6, ErrorMessage ="A senha precisa ter um número mínimo de 6 caracteres")]
        public string senha { get; set; }

    }
}
