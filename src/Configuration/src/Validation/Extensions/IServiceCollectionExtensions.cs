using Fetcharr.Configuration.Validation;
using Fetcharr.Configuration.Validation.Rules;

using Microsoft.Extensions.DependencyInjection;

namespace Fetcharr.Configuration.Extensions
{
    public static partial class IServiceCollectionExtensions
    {
        /// <summary>
        ///   Registers validation onto the given <see cref="IServiceCollection"/>.
        /// </summary>
        public static IServiceCollection AddValidation(this IServiceCollection services) =>
            services
                .AddScoped<IValidationPipeline, ValidationPipeline>()
                .AddDefaultValidationRules();

        /// <summary>
        ///   Registers default validation rules onto the given <see cref="IServiceCollection"/>.
        /// </summary>
        public static IServiceCollection AddDefaultValidationRules(this IServiceCollection services) =>
            services
                .AddValidationRule<PlexTokenValidationRule>()
                .AddValidationRule<ServiceValidationRule>();

        /// <summary>
        ///   Registers a validation rule onto the given <see cref="IServiceCollection"/>.
        /// </summary>
        public static IServiceCollection AddValidationRule<TRule>(this IServiceCollection services)
            where TRule : class, IValidationRule
            => services.AddScoped<IValidationRule, TRule>();
    }
}