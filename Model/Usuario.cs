using GerenciamentoDeEndereco.Model;
using Org.BouncyCastle.Crypto.Digests;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GerenciamentoDeEndereco.Model
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        public string nomeCompleto{ get; set; }

        public string nomeUsuario{ get; set; }

        public string senha { get; set; }

    }
}