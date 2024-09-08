using GraphQL;
using GraphQL.Client.Http;

namespace Fetcharr.Shared.GraphQL
{
    public static class GraphQLHttpClientExtensions
    {
        /// <summary>
        ///   Appends a default HTTP header to send along with all GraphQL operations.
        /// </summary>
        /// <param name="client"><see cref="GraphQLHttpClient"/>-instance to add the header onto.</param>
        /// <returns><paramref name="client"/> to allow for chaining calls.</returns>
        public static GraphQLHttpClient WithHeader(this GraphQLHttpClient client, string name, string value)
        {
            client.HttpClient.DefaultRequestHeaders.Add(name, value);
            return client;
        }

        /// <summary>
        ///   Enables Automatic Persisted Queries, when <paramref name="when"/> resolve to <see langword="true" />.
        /// </summary>
        /// <param name="client"><see cref="GraphQLHttpClient"/>-instance to enable APQ for.</param>
        /// <returns><paramref name="client"/> to allow for chaining calls.</returns>
        public static GraphQLHttpClient WithAutomaticPersistedQueries(
            this GraphQLHttpClient client,
            Func<GraphQLRequest, bool>? when = null)
        {
            when ??= _ => true;

            client.Options.EnableAutomaticPersistedQueries = when;
            return client;
        }
    }
}