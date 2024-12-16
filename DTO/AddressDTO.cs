using GerenciamentoDeEndereco.Model;
using GerenciamentoDeEndereco.Validators;

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
        /// Validates the input data for the address.
        /// This method ensures all address fields adhere to specified validation rules.
        /// </summary>
        public void ValidateData()
        {
            // Validate each address field using the ValidateInputDataAddress static methods.
            ValidateInputDataAddress.Zipcode(ZipCode);
            ValidateInputDataAddress.Street(Street);
            ValidateInputDataAddress.Neighborhood(Neighborhood);
            ValidateInputDataAddress.City(City);
            ValidateInputDataAddress.State(State);
            ValidateInputDataAddress.Number(Number);
        }
    }
}
