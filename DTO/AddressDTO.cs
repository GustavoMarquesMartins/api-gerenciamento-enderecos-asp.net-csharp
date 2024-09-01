using GerenciamentoDeEndereco.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GerenciamentoDeEndereco.DTO
{
    public class AddressDTO
    {
        [Required(ErrorMessage = "O campo cep não pode ficar em branco")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "O Cep deve conter 8 caracteres")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "O campo Logradouro não pode ficar em branco")]
        public string Street { get; set; }

        public string AdditionalInfo { get; set; }

        [Required(ErrorMessage = "O campo Bairro não pode ficar em branco")]
        public string Neighborhood { get; set; }

        [Required(ErrorMessage = "O campo Cidade não pode ficar em branco")]
        public string City { get; set; }

        [Required(ErrorMessage = "O campo UF não pode ficar em branco")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "O Uf deve conter 2 caracteres")]
        public string State { get; set; }

        [Required(ErrorMessage = "O campo Numero não pode ficar em branco")]
        public int Number { get; set; }
    }
}
