using Fetcharr.Models.Configuration;

namespace Fetcharr.Configuration.Validation.Rules
{
    public class PlexTokenValidationRule : IValidationRule
    {
        public ValidationResult Validate(FetcharrConfiguration configuration)
        {
            if(string.IsNullOrEmpty(configuration.Plex.ApiToken))
            {
                return new ValidationResult("`plex.api_token` must be set.");
            }

            return ValidationResult.Success;
        }
    }
}