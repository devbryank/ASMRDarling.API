namespace ASMRDarling.API.Helpers
{
    /// <summary>
    /// This class helps to build the media file source url
    /// </summary>
    class UrlBuilder
    {
        public const string MediaBaseUrl = "https://s3.amazonaws.com/asmr-darling-api-media";

        public static string GetS3FileSourceUrl(string fileName, string fileType)
        {
            return $"{MediaBaseUrl}/{fileType}/{fileName}.{fileType}";
        }
    }
}
