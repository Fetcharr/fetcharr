namespace Fetcharr.Provider.Plex.Models
{
    public class MediaResponse<TBody>
    {
        public MediaContainer<TBody> MediaContainer { get; set; } = new();
    }
}