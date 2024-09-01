using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace GerenciamentoDeEndereco.DTO
{
    public class AddressUpdateDTO
    {
        public string? ZipCode { get; set; }
        public string? Street { get; set; }
        public string? AdditionalInfo { get; set; }
        public string? Neighborhood { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public int Number { get; set; }

        public void validateDate()
        {
            if (ZipCode != null)
            {
                var padraoCep = @"^\d{8}$";


                if (!Regex.IsMatch(ZipCode, padraoCep)) throw new customException("O campo CEP deve conter 8 caractéres númericos sendo eles de 0-9!");
            }

            if (Street != null)
            {
                if (string.IsNullOrEmpty(Street)) throw new customException("O campo logradouro não pode ser passado em branco!");
            }

            if(Neighborhood != null)
            {
                if (string.IsNullOrEmpty(Neighborhood)) throw new customException("O campo logradouro não pode ser passado em branco!");
            }

            if (City != null)
            {
                var padraoCidade = @"^([A-Za-z]+\s?)+$";
                if (!Regex.IsMatch(City, padraoCidade)) throw new customException("O campo cidade deve conter apenas letras, não podendo conter números ou caractéres especias!");
            }

            if(State != null)
            {
                var padraoUf = @"^[A-Z]{2}$";
                if (!Regex.IsMatch(State, padraoUf)) throw new customException("O campo UF deve conter ser representado apenas por duas letras!");
            }

            if (Number != null)
            {
                var padraoNumero = @"^\d{1,}$";
                if (!Regex.IsMatch(Number.ToString(), padraoNumero)) throw new customException("O campo número não pode conter letras!");
            }
        }
    }

    public class customException : Exception
    {
        public customException(String erro) : base("Erro : " + erro)
        { }
    }
}
