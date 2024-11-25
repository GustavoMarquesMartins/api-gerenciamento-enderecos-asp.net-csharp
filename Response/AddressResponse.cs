namespace GerenciamentoDeEndereco.Response
{
    /// <summary>
    /// Response model for address data.
    /// </summary>
    public class AddressResponse
    {
        /// <summary>
        /// Gets or sets the address ID.
        /// </summary>
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
    }
}
