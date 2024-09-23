using Cachedeer;

using Fetcharr.Models.Configuration;
using Fetcharr.Provider.Plex.Models;
using Fetcharr.Shared.GraphQL;

using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fetcharr.Provider.Plex.Clients
{
    /// <summary>
    ///   Client for interacting with Plex' GraphQL API.
    /// </summary>
    public class PlexGraphQLClient(
        IOptions<FetcharrConfiguration> configuration,
        [FromKeyedServices("watchlist")] ICachingProvider watchlistCachingProvider)
    {
        /// <summary>
        ///   Gets the GraphQL endpoint for Plex.
        /// </summary>
        public const string GraphQLEndpoint = "https://community.plex.tv/api";

        private readonly GraphQLHttpClient _client =
            new GraphQLHttpClient(PlexGraphQLClient.GraphQLEndpoint, new SystemTextJsonSerializer())
                .WithAutomaticPersistedQueries(_ => true)
                .WithHeader("X-Plex-Token", configuration.Value.Plex.ApiToken)
                .WithHeader("X-Plex-Client-Identifier", "fetcharr");

        /// <summary>
        ///   Gets the watchlist of a Plex account, who's a friend of the current plex account.
        /// </summary>
        public async Task<IEnumerable<WatchlistMetadataItem>> GetFriendWatchlistAsync(
            string userId,
            int count = 100,
            string? cursor = null)
        {
            string cacheKey = $"friend-watchlist-{userId}";

            CacheItem<IEnumerable<WatchlistMetadataItem>> cachedResponse = await watchlistCachingProvider
                .GetAsync<IEnumerable<WatchlistMetadataItem>>(cacheKey);

            if(cachedResponse.HasValue)
            {
                return cachedResponse.Value;
            }

            GraphQLRequest request = new()
            {
                Query = """
                query GetFriendWatchlist($uuid: ID = "", $first: PaginationInt!, $after: String) {
                    user(id: $uuid) {
                        watchlist(first: $first, after: $after) {
                            nodes {
                                ... on MetadataItem {
                                    title
                                    ratingKey: id
                                    year
                                    type
                                }
                            }
                            pageInfo {
                                hasNextPage
                                endCursor
                            }
                        }
                    }
                }
                """,
                OperationName = "GetFriendWatchlist",
                Variables = new
                {
                    uuid = userId,
                    first = count,
                    after = cursor ?? string.Empty
                }
            };

            GraphQLResponse<PlexUserWatchlistResponseType> response = await this._client
                .SendQueryAsync<PlexUserWatchlistResponseType>(request);

            response.ThrowIfErrors(message: "Failed to fetch friend's watchlist from Plex");

            IEnumerable<WatchlistMetadataItem> watchlistItems = response.Data.User.Watchlist.Nodes;

            await watchlistCachingProvider.SetAsync(cacheKey, watchlistItems, expiration: TimeSpan.FromHours(4));
            return watchlistItems;
        }

        /// <summary>
        ///   Gets all the friends of the current Plex account and returns them.
        /// </summary>
        public async Task<IEnumerable<PlexFriendUser>> GetAllFriendsAsync()
        {
            CacheItem<IEnumerable<PlexFriendUser>> cachedResponse = await watchlistCachingProvider
                .GetAsync<IEnumerable<PlexFriendUser>>("friends-list");

            if(cachedResponse.HasValue)
            {
                return cachedResponse.Value;
            }

            GraphQLRequest request = new()
            {
                Query = """
                query {
                    allFriendsV2 {
                        user {
                            id
                            username
                        }
                    }
                }
                """
            };

            GraphQLResponse<PlexFriendListResponseType> response = await this._client
                .SendQueryAsync<PlexFriendListResponseType>(request);

            response.ThrowIfErrors(message: "Failed to fetch friends list from Plex");

            IEnumerable<PlexFriendUser> friends = response.Data.Friends.Select(v => v.User);

            await watchlistCachingProvider.SetAsync("friends-list", friends, expiration: TimeSpan.FromHours(4));
            return friends;
        }
    }
}