namespace GerenciamentoDeEndereco.Response
{
    public class UsuarioResponse
    {
        public string nomeCompleto { get; set; }
        public string email { get; set; }
        public string senha { get; set; }

        public UsuarioResponse(string nome, string email, string senha)
        {
            nomeCompleto = nome;
            email = email;
            senha = senha;
        }
        
        public UsuarioResponse(){}
    }

}
