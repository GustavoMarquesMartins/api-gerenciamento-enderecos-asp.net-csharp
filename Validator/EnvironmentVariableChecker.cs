namespace GerenciamentoDeEndereco.Validator
{
    public class EnvironmentVariableChecker
    {

        public static readonly string[] RequiredEnvVariables =
             {
                "SECRET_KEY_APPLICATION",
                "DATABASE_HOST",
                "DATABASE_PORT",
                "DATABASE_NAME",
                "DATABASE_USER",
                "DATABASE_PASSWORD",
                "SMTP_EMAIL",
                "SMTP_APP_PASSWORD"
            };

        public static void Validate()
        {
            // Validate environment variables
            foreach (var variable in RequiredEnvVariables)
            {
                if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(variable)))
                {
                    throw new InvalidOperationException($"The required environment variable '{variable}' is not set.");
                }
            }
        }
    }
}
