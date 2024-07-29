namespace Fetcharr.Configuration.Exceptions
{
    public class DuplicateServiceKeyException(string name, string service)
        : Exception($"Duplicate {service} name in configuration: '{name}'")
    {
    }
}