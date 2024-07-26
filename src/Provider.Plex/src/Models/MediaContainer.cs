namespace Fetcharr.Provider.Plex.Models
{
    public class MediaContainer<TBody>
    {
        public string Identifier { get; set; } = string.Empty;

        public string LibrarySectionID { get; set; } = string.Empty;

        public string LibrarySectionTitle { get; set; } = string.Empty;

        public int Size { get; set; }

        public int Offset { get; set; }

        public int TotalSize { get; set; }

        public IEnumerable<TBody> Metadata { get; set; } = [];
    }
}