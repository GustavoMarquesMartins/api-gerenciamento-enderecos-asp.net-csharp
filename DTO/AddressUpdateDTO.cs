using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using GerenciamentoDeEndereco.Validators;

namespace GerenciamentoDeEndereco.DTO
{
    /// <summary>
    /// Data Transfer Object for updating address details.
    /// </summary>
    public class AddressUpdateDTO
    {
        /// <summary>
        /// Gets or sets the ZIP code.
        /// </summary>
        public string? ZipCode { get; set; }

        /// <summary>
        /// Gets or sets the street name.
        /// </summary>
        public string? Street { get; set; }

        /// <summary>
        /// Gets or sets the additional information.
        /// </summary>
        public string? AdditionalInfo { get; set; }

        /// <summary>
        /// Gets or sets the neighborhood name.
        /// </summary>
        public string? Neighborhood { get; set; }

        /// <summary>
        /// Gets or sets the city name.
        /// </summary>
        public string? City { get; set; }

        /// <summary>
        /// Gets or sets the state name.
        /// </summary>
        public string? State { get; set; }

        /// <summary>
        /// Gets or sets the address number.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Validates the fields for correct format using custom validators.
        /// This method ensures that only non-null fields are validated.
        /// </summary>
        /// <exception cref="Exception">Thrown when a field is invalid.</exception>
        public void ValidateData()
        {
            // Validate ZIP code if it is not null
            if (ZipCode != null)
            {
                ValidateInputDataAddress.Zipcode(ZipCode);
            }

            // Validate street name if it is not null
            if (Street != null)
            {
                ValidateInputDataAddress.Street(Street);
            }

            // Validate neighborhood name if it is not null
            if (Neighborhood != null)
            {
                ValidateInputDataAddress.Neighborhood(Neighborhood);
            }

            // Validate city name if it is not null
            if (City != null)
            {
                ValidateInputDataAddress.City(City);
            }

            // Validate state abbreviation if it is not null
            if (State != null)
            {
                ValidateInputDataAddress.State(State);
            }

            // Validate address number if it is not zero
            if (Number != 0)
            {
                ValidateInputDataAddress.Number(Number);
            }
        }
    }
}
