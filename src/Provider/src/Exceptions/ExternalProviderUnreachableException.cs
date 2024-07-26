namespace Fetcharr.Provider.Exceptions
{
    [System.Serializable]
    public class ExternalProviderUnreachableException
        : Exception
    {
        private const string MessageFormat = "Provider cannot be reached: {0}";

        public readonly string ProviderName;

        public ExternalProviderUnreachableException(string providerName)
            : base(string.Format(MessageFormat, providerName))
            => this.ProviderName = providerName;

        public ExternalProviderUnreachableException(string providerName, Exception inner)
            : base(string.Format(MessageFormat, providerName), inner)
            => this.ProviderName = providerName;
    }
}