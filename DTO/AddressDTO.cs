using GerenciamentoDeEndereco.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GerenciamentoDeEndereco.DTO
{
    /// <summary>
    /// Data Transfer Object for address creation.
    /// </summary>
    public class AddressDTO
    {
        /// <summary>
        /// Gets or sets the ZIP code.
        /// </summary>
        [Required(ErrorMessage = "The ZIP code field cannot be blank")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "The ZIP code must be 8 characters long")]
        public string ZipCode { get; set; }

        /// <summary>
        /// Gets or sets the street name.
        /// </summary>
        [Required(ErrorMessage = "The street field cannot be blank")]
        public string Street { get; set; }

        /// <summary>
        /// Gets or sets the additional information.
        /// </summary>
        public string AdditionalInfo { get; set; }

        /// <summary>
        /// Gets or sets the neighborhood name.
        /// </summary>
        [Required(ErrorMessage = "The neighborhood field cannot be blank")]
        public string Neighborhood { get; set; }

        /// <summary>
        /// Gets or sets the city name.
        /// </summary>
        [Required(ErrorMessage = "The city field cannot be blank")]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the state name.
        /// </summary>
        [Required(ErrorMessage = "The state field cannot be blank")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "The state (UF) must be 2 characters long")]
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the address number.
        /// </summary>
        [Required(ErrorMessage = "The number field cannot be blank")]
        public int Number { get; set; }
    }
}
