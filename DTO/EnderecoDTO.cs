using GerenciamentoDeEndereco.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GerenciamentoDeEndereco.DTO
{
    public class EnderecoDTO
    {
        [Required(ErrorMessage = "O campo cep não pode ficar em branco")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "O Cep deve conter 8 caracteres")]
        public string cep { get; set; }

        [Required(ErrorMessage = "O campo Logradouro não pode ficar em branco")]
        public string logradouro { get; set; }

        public string complemento { get; set; }

        [Required(ErrorMessage = "O campo Bairro não pode ficar em branco")]
        public string bairro { get; set; }

        [Required(ErrorMessage = "O campo Cidade não pode ficar em branco")]
        public string cidade { get; set; }

        [Required(ErrorMessage = "O campo UF não pode ficar em branco")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "O Uf deve conter 2 caracteres")]
        public string uf { get; set; }

        [Required(ErrorMessage = "O campo Numero não pode ficar em branco")]
        public int numero { get; set; }

        [Required(ErrorMessage = "O campo usuarioId não pode ficar em branco")]
        public long usuarioId { get; set; }
    }
}
