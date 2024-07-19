using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace GerenciamentoDeEndereco.DTO
{
    public class EnderecoEdicaoDTO
    {
        public string? cep { get; set; }

        public string? logradouro { get; set; }

        public string? complemento { get; set; }

        public string? bairro { get; set; }

        public string? cidade { get; set; }

        public string? uf { get; set; }

        public int numero { get; set; }

        public void validateDate()
        {
            if (cep != null)
            {
                var padraoCep = @"^\d{8}$";
                Console.Write(cep);
                Console.Write(cep);
                Console.Write(cep);
                Console.Write(cep);

                if (!Regex.IsMatch(cep, padraoCep)) throw new customException("O campo CEP deve conter 8 caractéres númericos sendo eles de 0-9!");
            }

            if (logradouro != null)
            {
                if (string.IsNullOrEmpty(logradouro)) throw new customException("O campo logradouro não pode ser passado em branco!");
            }

            if(bairro != null)
            {
                if (string.IsNullOrEmpty(bairro)) throw new customException("O campo logradouro não pode ser passado em branco!");
            }

            if (cidade != null)
            {
                var padraoCidade = @"^([A-Za-z]+\s?)+$";
                if (!Regex.IsMatch(cidade, padraoCidade)) throw new customException("O campo cidade deve conter apenas letras, não podendo conter números ou caractéres especias!");
            }

            if(uf != null)
            {
                var padraoUf = @"^[A-Z]{2}$";
                if (!Regex.IsMatch(uf, padraoUf)) throw new customException("O campo UF deve conter ser representado apenas por duas letras!");
            }

            if (numero != null)
            {
                var padraoNumero = @"^\d{1,}$";
                if (!Regex.IsMatch(numero.ToString(), padraoNumero)) throw new customException("O campo número não pode conter letras!");
            }
        }
    }

    public class customException : Exception
    {
        public customException(String erro) : base("Erro : " + erro)
        { }
    }
}
