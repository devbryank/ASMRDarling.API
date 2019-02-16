namespace ASMRDarling.API.Helpers
{
    /// <summary>
    /// this class helps to build the media item source url
    /// </summary>
    class UrlHelper
    {
        const string MediaBaseUrl = "https://s3.amazonaws.com/asmr-darling-api-media";

        public static string GetMediaSourceUrl(string fileName, string fileType)
        {
            return $"{MediaBaseUrl}/{fileType}/{fileName}.{fileType}";
        }
    }
}
