using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

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
        /// Validates the fields for correct format.
        /// </summary>
        /// <exception cref="customException">Thrown when a field is invalid</exception>
        public void ValidateDate()
        {
            if (ZipCode != null)
            {
                var zipCodePattern = @"^\d{8}$";

                if (!Regex.IsMatch(ZipCode, zipCodePattern))
                    throw new customException("The ZIP code field must contain 8 numeric characters from 0-9.");
            }

            if (Street != null)
            {
                if (string.IsNullOrEmpty(Street))
                    throw new customException("The street field cannot be blank.");
            }

            if (Neighborhood != null)
            {
                if (string.IsNullOrEmpty(Neighborhood))
                    throw new customException("The neighborhood field cannot be blank.");
            }

            if (City != null)
            {
                var cityPattern = @"^([A-Za-z]+\s?)+$";
                if (!Regex.IsMatch(City, cityPattern))
                    throw new customException("The city field should contain only letters, no numbers or special characters.");
            }

            if (State != null)
            {
                var statePattern = @"^[A-Z]{2}$";
                if (!Regex.IsMatch(State, statePattern))
                    throw new customException("The state (UF) field must be represented by two letters.");
            }

            if (Number != 0)
            {
                var numberPattern = @"^\d{1,}$";
                if (!Regex.IsMatch(Number.ToString(), numberPattern))
                    throw new customException("The number field cannot contain letters.");
            }
        }
    }

    /// <summary>
    /// Exception thrown when a custom validation error occurs.
    /// </summary>
    public class customException : Exception
    {
        public customException(string error) : base("Error: " + error)
        { }
    }
}
