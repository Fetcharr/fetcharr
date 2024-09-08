using Fetcharr.Provider.Plex.Clients;
using Fetcharr.Provider.Plex.Models;

namespace Fetcharr.Provider.Plex
{
    /// <summary>
    ///   Client for fetching friends' watchlists from Plex.
    /// </summary>
    public class PlexFriendsWatchlistClient(
        PlexGraphQLClient plexGraphQLClient)
    {
        /// <summary>
        ///   Fetch the watchlists for all the friends the current Plex account and return them.
        /// </summary>
        /// <param name="count">Maximum amount of items to fetch per watchlist.</param>
        public async Task<IEnumerable<WatchlistMetadataItem>> FetchAllWatchlistsAsync(int count = 10)
        {
            List<WatchlistMetadataItem> joinedWatchlist = [];
            IEnumerable<PlexFriendUser> friends = await plexGraphQLClient.GetAllFriendsAsync();

            foreach(PlexFriendUser friend in friends)
            {
                IEnumerable<WatchlistMetadataItem> friendWatchlist = await plexGraphQLClient
                    .GetFriendWatchlistAsync(friend.Id, count);

                joinedWatchlist.AddRange(friendWatchlist);
            }

            return joinedWatchlist;
        }
    }
}