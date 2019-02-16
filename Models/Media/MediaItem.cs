namespace ASMRDarling.API.Models
{
    /// <summary>
    /// base model for the media item
    /// </summary>
    class MediaItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Length { get; set; }
        public string FileName { get; set; }
        public string Thumbnail { get; set; }
        public string AudioSource { get; set; }
        public string VideoSource { get; set; }
    }
}
