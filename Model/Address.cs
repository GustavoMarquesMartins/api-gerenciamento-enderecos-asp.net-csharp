using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GerenciamentoDeEndereco.Model
{
    /// <summary>
    /// Model class representing an Address entity.
    /// </summary>
    public class Address
    {
        /// <summary>
        /// Gets or sets the address ID.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the ZIP code.
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// Gets or sets the street name.
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// Gets or sets the additional information.
        /// </summary>
        public string AdditionalInfo { get; set; }

        /// <summary>
        /// Gets or sets the neighborhood name.
        /// </summary>
        public string Neighborhood { get; set; }

        /// <summary>
        /// Gets or sets the city name.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the state name.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the address number.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Gets or sets the user ID for the address.
        /// </summary>
        [ForeignKey("UserId")]
        public long UserId { get; set; }
    }
}
