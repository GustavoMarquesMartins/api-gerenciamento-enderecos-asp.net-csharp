using System.Security.Cryptography;

namespace GerenciamentoDeEndereco.Infra
{
    public static class TokenGenerator
    {
        // Método para gerar um token seguro
        public static string GenerateToken(int length = 32)
        {
            // Cria uma instância do gerador de números aleatórios
            using (var rng = new RNGCryptoServiceProvider())
            {
                // Cria um array de bytes com o comprimento desejado
                var bytes = new byte[length];

                // Preenche o array de bytes com dados aleatórios
                rng.GetBytes(bytes);

                // Converte o array de bytes para uma string Base64 e retorna
                return Convert.ToBase64String(bytes).Replace(" ","");
            }
        }
    }
}