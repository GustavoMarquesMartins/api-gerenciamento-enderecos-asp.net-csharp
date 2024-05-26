namespace GerenciamentoDeEndereco.Response
{
    public class UsuarioResponse
    {
        public string nomeCompleto { get; set; }
        public string nomeUsuario { get; set; }
        public string senha { get; set; }
        public UsuarioResponse(string nome, string nomeUsuario, string senha)
        {
            nome = nome;
            nomeUsuario = nomeUsuario;
            senha = senha;
        }
        public UsuarioResponse(){}
    }

}
