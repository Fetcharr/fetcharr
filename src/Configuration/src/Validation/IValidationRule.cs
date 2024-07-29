using Fetcharr.Models.Configuration;

namespace Fetcharr.Configuration.Validation
{
    /// <summary>
    ///   Representation of a validation rule for configuration files.
    /// </summary>
    public interface IValidationRule
    {
        /// <summary>
        ///   Validates the given configuration file and returns the result.
        /// </summary>
        ValidationResult Validate(FetcharrConfiguration configuration);
    }
}