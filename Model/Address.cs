using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GerenciamentoDeEndereco.Model
{
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string ZipCode { get; set; } 
        public string Street { get; set; } 
        public string AdditionalInfo { get; set; } 
        public string Neighborhood { get; set; } 
        public string City { get; set; }
        public string State { get; set; } 
        public int Number { get; set; }

        [ForeignKey("UserId")]
        public long UserId { get; set; } 

    }
}
