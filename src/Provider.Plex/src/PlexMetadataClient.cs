using System.Text.Json;

using Fetcharr.Cache.Core;
using Fetcharr.Models.Configuration;
using Fetcharr.Provider.Plex.Models;

using Flurl.Http;
using Flurl.Http.Configuration;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fetcharr.Provider.Plex
{
    /// <summary>
    ///   Client for fetching metadata from Plex.
    /// </summary>
    public class PlexMetadataClient(
        IOptions<FetcharrConfiguration> configuration,
        [FromKeyedServices("metadata")] ICachingProvider cachingProvider)
    {
        private readonly FlurlClient _client =
            new FlurlClient("https://metadata.provider.plex.tv/library/metadata/")
                .WithHeader("X-Plex-Token", configuration.Value.Plex.ApiToken)
                .WithHeader("X-Plex-Client-Identifier", "fetcharr")
                .WithSettings(opts => opts.JsonSerializer = new DefaultJsonSerializer(new JsonSerializerOptions
                {
                    // Metadata endpoints send back an object with both 'guid' and 'Guid' keys,
                    // so we need to explicitly name all properties within the models...
                    PropertyNameCaseInsensitive = false,
                }));

        /// <summary>
        ///   Gets metadata from Plex for an item, given it's rating key, <paramref name="ratingKey"/>.
        /// </summary>
        public async Task<PlexMetadataItem?> GetMetadataFromRatingKeyAsync(string ratingKey)
        {
            MediaResponse<PlexMetadataItem> metadata = await cachingProvider.GetAsync<MediaResponse<PlexMetadataItem>>(
                ratingKey,
                async () => await this._client
                    .Request(ratingKey)
                    .AppendQueryParam("format", "json")
                    .GetJsonAsync<MediaResponse<PlexMetadataItem>>());

            return metadata.MediaContainer.Metadata?.FirstOrDefault();
        }
    }
}