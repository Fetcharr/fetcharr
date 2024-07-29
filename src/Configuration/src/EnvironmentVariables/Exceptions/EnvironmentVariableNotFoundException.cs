namespace Fetcharr.Configuration.EnvironmentVariables.Exceptions
{
    public class EnvironmentVariableNotFoundException : Exception
    {
        private const string ExceptionFormat = "Environment variable '{0}' was referenced, but was not defined and has no default value.";

        public EnvironmentVariableNotFoundException(string variable)
            : base(string.Format(ExceptionFormat, variable))
        {

        }

        public EnvironmentVariableNotFoundException(string variable, Exception innerException)
            : base(string.Format(ExceptionFormat, variable), innerException)
        {

        }
    }
}