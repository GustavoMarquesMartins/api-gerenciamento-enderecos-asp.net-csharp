namespace GerenciamentoDeEndereco.Security.Generator
{
    public class VerificationCodeGenerator
    {
        /// <summary>
        /// Generates a unique 6-digit verification code.
        /// </summary>
        /// <returns>A unique verification code.</returns>
        public static string GenerateVerificationCode()
        {
            Random random = new Random();
            string verificationCode;

            verificationCode = random.Next(100000, 1000000).ToString();

            return verificationCode;
        }
    }
}
