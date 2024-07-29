using Fetcharr.Models.Configuration;
using Fetcharr.Models.Configuration.Radarr;
using Fetcharr.Models.Configuration.Sonarr;

namespace Fetcharr.Configuration.Validation.Rules
{
    public class ServiceValidationRule : IValidationRule
    {
        public ValidationResult Validate(FetcharrConfiguration configuration)
        {
            foreach(KeyValuePair<string, FetcharrRadarrConfiguration> instance in configuration.Radarr)
            {
                if(string.IsNullOrEmpty(instance.Value.BaseUrl))
                {
                    return new ValidationResult($"`radarr.{instance.Key}.base_url` must be set.");
                }

                if(string.IsNullOrEmpty(instance.Value.ApiKey))
                {
                    return new ValidationResult($"`radarr.{instance.Key}.api_key` must be set.");
                }
            }

            foreach(KeyValuePair<string, FetcharrSonarrConfiguration> instance in configuration.Sonarr)
            {
                if(string.IsNullOrEmpty(instance.Value.BaseUrl))
                {
                    return new ValidationResult($"`sonarr.{instance.Key}.base_url` must be set.");
                }

                if(string.IsNullOrEmpty(instance.Value.ApiKey))
                {
                    return new ValidationResult($"`sonarr.{instance.Key}.api_key` must be set.");
                }
            }

            return ValidationResult.Success;
        }
    }
}