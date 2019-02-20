namespace Sonnar.Helpers
{
    class UrlHelper
    {
        const string MediaBaseUrl = "https://s3.amazonaws.com/asmr-darling-api-media";


        public static string GetSourceUrl(string fileName, string fileType)
        {
            return $"{MediaBaseUrl}/{fileType}/{fileName}.{fileType}";
        }
    }
}
