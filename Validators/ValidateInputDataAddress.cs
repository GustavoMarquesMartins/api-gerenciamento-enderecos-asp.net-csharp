using System.Text.RegularExpressions;

namespace GerenciamentoDeEndereco.Validators
{
    /// <summary>
    /// Validator class for address input data.
    /// </summary>
    public static class ValidateInputDataAddress
    {
        /// <summary>
        /// Validates the ZIP code.
        /// </summary>
        /// <param name="zipcode">The ZIP code to validate.</param>
        /// <exception cref="Exception">Thrown when the ZIP code is invalid.</exception>
        public static void Zipcode(string zipcode)
        {
            // Define the regex pattern for a valid ZIP code (8 numeric characters)
            var zipCodePattern = @"^\d{8}$";
            // Validate the ZIP code against the pattern
            if (!Regex.IsMatch(zipcode, zipCodePattern))
                throw new ArgumentException("The ZIP code field must contain 8 numeric characters from 0-9.");
        }

        /// <summary>
        /// Validates the street name.
        /// </summary>
        /// <param name="street">The street name to validate.</param>
        /// <exception cref="Exception">Thrown when the street name is blank.</exception>
        public static void Street(string street)
        {
            // Validate that the street name is not blank
            if (string.IsNullOrEmpty(street))
                throw new ArgumentException("The street field cannot be blank.");
        }

        /// <summary>
        /// Validates the neighborhood name.
        /// </summary>
        /// <param name="neighborhood">The neighborhood name to validate.</param>
        /// <exception cref="Exception">Thrown when the neighborhood name is blank.</exception>
        public static void Neighborhood(string neighborhood)
        {
            // Validate that the neighborhood name is not blank
            if (string.IsNullOrEmpty(neighborhood))
                throw new ArgumentException("The neighborhood field cannot be blank.");
        }

        /// <summary>
        /// Validates the city name.
        /// </summary>
        /// <param name="city">The city name to validate.</param>
        /// <exception cref="Exception">Thrown when the city name is invalid.</exception>
        public static void City(string city)
        {
            // Define the regex pattern for a valid city name (only letters)
            var cityPattern = @"^([A-Za-z]+\s?)+$";
            // Validate the city name against the pattern
            if (!Regex.IsMatch(city, cityPattern))
                throw new ArgumentException("The city field should contain only letters, no numbers or special characters.");
        }

        /// <summary>
        /// Validates the state abbreviation.
        /// </summary>
        /// <param name="state">The state abbreviation to validate.</param>
        /// <exception cref="Exception">Thrown when the state abbreviation is invalid.</exception>
        public static void State(string state)
        {
            // Define the regex pattern for a valid state abbreviation (2 letters)
            var statePattern = @"^[A-Z]{2}$";
            // Validate the state abbreviation against the pattern
            if (!Regex.IsMatch(state, statePattern))
                throw new ArgumentException("The state (UF) field must be represented by two letters.");
        }

        /// <summary>
        /// Validates the address number.
        /// </summary>
        /// <param name="number">The address number to validate.</param>
        /// <exception cref="Exception">Thrown when the address number is invalid.</exception>
        public static void Number(int number)
        {
            // Define the regex pattern for a valid address number (only numeric characters)
            var numberPattern = @"^\d{1,}$";
            // Validate the address number against the pattern
            if (!Regex.IsMatch(number.ToString(), numberPattern))
                throw new ArgumentException("The number field cannot contain letters.");
        }
    }
}
