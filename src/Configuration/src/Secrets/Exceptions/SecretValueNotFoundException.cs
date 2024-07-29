namespace Fetcharr.Configuration.EnvironmentVariables.Exceptions
{
    public class SecretValueNotFoundException : Exception
    {
        private const string ExceptionFormat = "Secret value '{0}' was referenced, but was not defined.";

        public SecretValueNotFoundException(string variable)
            : base(string.Format(ExceptionFormat, variable))
        {

        }

        public SecretValueNotFoundException(string variable, Exception innerException)
            : base(string.Format(ExceptionFormat, variable), innerException)
        {

        }
    }
}