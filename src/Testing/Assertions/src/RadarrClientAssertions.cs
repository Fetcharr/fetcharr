using System.Diagnostics.CodeAnalysis;

using Fetcharr.Provider.Radarr;
using Fetcharr.Provider.Radarr.Models;

using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

using static System.StringComparison;

namespace Fetcharr.Testing.Assertions
{
    public class RadarrClientAssertions(RadarrClient instance)
        : ReferenceTypeAssertions<RadarrClient, RadarrClientAssertions>(instance)
    {
        protected override string Identifier => "radarr client";

        [CustomAssertion]
        public async Task<AndConstraint<RadarrClientAssertions>> ContainMovieAsync(
            string movieTitle,
            [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
        {
            bool success = Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(!string.IsNullOrEmpty(movieTitle))
                .FailWith("You can't assert a movie's existence if you don't pass a proper title");

            if(success)
            {
                IEnumerable<RadarrMovie> movies = await this.Subject.GetAllMoviesAsync();

                Execute.Assertion
                    .ForCondition(movies.Any(v => v.Title.Equals(movieTitle, InvariantCultureIgnoreCase)))
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:movies} {0} to have movie with the title of {1}{reason}.",
                        movies.Select(v => v.Title), movieTitle);
            }

            return new AndConstraint<RadarrClientAssertions>(this);
        }

        [CustomAssertion]
        public async Task<AndConstraint<RadarrClientAssertions>> NotContainMovieAsync(
            string movieTitle,
            [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
        {
            bool success = Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(!string.IsNullOrEmpty(movieTitle))
                .FailWith("You can't assert a movie's existence if you don't pass a proper title");

            if(success)
            {
                IEnumerable<RadarrMovie> movies = await this.Subject.GetAllMoviesAsync();

                Execute.Assertion
                    .ForCondition(!movies.Any(v => v.Title.Equals(movieTitle, InvariantCultureIgnoreCase)))
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Did not expect {context:movies} {0} to have movie with the title of {1}{reason}.",
                        movies.Select(v => v.Title), movieTitle);
            }

            return new AndConstraint<RadarrClientAssertions>(this);
        }
    }
}