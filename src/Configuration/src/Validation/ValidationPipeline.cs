using Fetcharr.Models.Configuration;

using Microsoft.Extensions.Logging;

namespace Fetcharr.Configuration.Validation
{
    /// <summary>
    ///   Representation of a validation pipeline for configuration files.
    /// </summary>
    public interface IValidationPipeline
    {
        /// <summary>
        ///   Validates the given configuration file.
        ///   If any validation errors occur, they are logged and method returns <see langword="false" />.
        ///   Otherwise, returns <see langword="true" />.
        /// </summary>
        bool Validate(FetcharrConfiguration configuration);
    }

    /// <summary>
    ///   Default implementation of <see cref="IValidationPipeline" />.
    /// </summary>
    public class ValidationPipeline(
        IEnumerable<IValidationRule> rules,
        ILogger<ValidationPipeline> logger)
        : IValidationPipeline
    {
        public bool Validate(FetcharrConfiguration configuration)
        {
            List<ValidationResult> failures = [];

            foreach(IValidationRule rule in rules)
            {
                ValidationResult result = rule.Validate(configuration);
                if(!result.IsSuccess)
                {
                    failures.Add(result);
                    logger.LogCritical("Config validation error: {Error}", result.ErrorMessage);
                }
            }

            return failures.Count == 0;
        }
    }
}