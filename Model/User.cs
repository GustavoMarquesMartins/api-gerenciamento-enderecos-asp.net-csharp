using GerenciamentoDeEndereco.Model;
using Org.BouncyCastle.Crypto.Digests;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GerenciamentoDeEndereco.Model
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Name{ get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

    }
}