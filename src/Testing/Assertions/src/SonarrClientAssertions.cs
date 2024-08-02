using System.Diagnostics.CodeAnalysis;

using Fetcharr.Provider.Sonarr;
using Fetcharr.Provider.Sonarr.Models;

using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

using static System.StringComparison;

namespace Fetcharr.Testing.Assertions
{
    public class SonarrClientAssertions(SonarrClient instance)
        : ReferenceTypeAssertions<SonarrClient, SonarrClientAssertions>(instance)
    {
        protected override string Identifier => "sonarr client";

        [CustomAssertion]
        public async Task<AndConstraint<SonarrClientAssertions>> ContainSeriesAsync(
            string movieTitle,
            [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
        {
            bool success = Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(!string.IsNullOrEmpty(movieTitle))
                .FailWith("You can't assert a series' existence if you don't pass a proper title");

            if(success)
            {
                IEnumerable<SonarrSeries> series = await this.Subject.GetAllSeriesAsync();

                Execute.Assertion
                    .ForCondition(series.Any(v => v.Title.Equals(movieTitle, InvariantCultureIgnoreCase)))
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:series} {0} to have series with the title of {1}{reason}.",
                        series.Select(v => v.Title), movieTitle);
            }

            return new AndConstraint<SonarrClientAssertions>(this);
        }

        [CustomAssertion]
        public async Task<AndConstraint<SonarrClientAssertions>> NotContainSeriesAsync(
            string movieTitle,
            [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
        {
            bool success = Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(!string.IsNullOrEmpty(movieTitle))
                .FailWith("You can't assert a series' existence if you don't pass a proper title");

            if(success)
            {
                IEnumerable<SonarrSeries> series = await this.Subject.GetAllSeriesAsync();

                Execute.Assertion
                    .ForCondition(!series.Any(v => v.Title.Equals(movieTitle, InvariantCultureIgnoreCase)))
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Did not expect {context:series} {0} to have series with the title of {1}{reason}.",
                        series.Select(v => v.Title), movieTitle);
            }

            return new AndConstraint<SonarrClientAssertions>(this);
        }
    }
}