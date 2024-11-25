namespace GerenciamentoDeEndereco.Response
{
    /// <summary>
    /// Response model for user data.
    /// </summary>
    public class UserResponse
    {
        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the user email.
        /// </summary>
        public string Email { get; set; }
    }
}
