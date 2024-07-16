namespace GerenciamentoDeEndereco.Response
{
    public class EnderecoResponse
    {
        public long id { get; set; }
        public string Cep { get; set; }
        public string logradouro { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
        public int Numero { get; set; }
    }
}
