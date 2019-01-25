namespace ASMRDarling.API.Builders
{
    public class UrlBuilder
    {
        public static string GetS3FileSourceUrl(string root, string fileName, string fileType)
        {
            return $"{root}/{fileType}/{fileName}.{fileType}";
        }
    }
}
