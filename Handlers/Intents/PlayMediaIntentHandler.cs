using Amazon.Lambda.Core;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Alexa.NET.Response.Directive;
using ASMRDarling.API.Builders;
using ASMRDarling.API.Interfaces;

namespace ASMRDarling.API.Handlers
{
    class PlayMediaIntentHandler : IPlayMediaIntentHandler
    {
        // Source related constants
        public const string MediaFileSlotName = "MediaFileName";
        public const string MediaBaseUrl = "https://s3.amazonaws.com/asmr-darling-api-media";


        // Constructor
        public PlayMediaIntentHandler() { }


        // Intent handler
        public async Task<SkillResponse> HandleIntent(Intent intent, Session session, ILambdaLogger logger)
        {
            logger.LogLine($"[PlayMediaIntentHandler.HandleIntent()] Play media intent handling started");

#warning resolution should support multiple slots

            // Get slot values
            Slot slot = intent.Slots[MediaFileSlotName];
            string slotValue = slot.Value;
            logger.LogLine($"[PlayMediaIntentHandler.HandleIntent()] Requested slot value (synonym): {slotValue}");

            // Get resolution, multiple slots in a dialog will cause an exception
            ResolutionAuthority[] resolution = slot.Resolution.Authorities;
            ResolutionValueContainer[] container = resolution[0].Values;

            // Get file extentions based on the display availability
            bool? hasDisplay = session.Attributes["has_display"] as bool?;
            string fileType = hasDisplay == true ? "mp4" : "mp3";

            // Convert file name into lower cases without any white spaces
            var fileName = Regex.Replace(container[0].Value.Name, @"\s", "").ToLower();
            logger.LogLine($"[PlayMediaIntentHandler.HandleIntent()] Media file requested: {fileName}.{fileType}");

            // Get file source url
            string url = UrlBuilder.GetS3FileSourceUrl(MediaBaseUrl, fileName, fileType);
            logger.LogLine($"[PlayMediaIntentHandler.HandleIntent()] Media file source URL: {url}");

            // Declare response to return
            SkillResponse response = new SkillResponse();

            if (hasDisplay == true)
            {
                // If the device has display
                logger.LogLine($"[PlayMediaIntentHandler.HandleIntent()] Generating a video app response");

#warning video app or apl video? which one is better?

                // Return video app response
                response = ResponseBuilder.Empty();
                response.Response.Directives.Add(new VideoAppDirective(url));
            }
            else
            {
                // If display is not available
                logger.LogLine($"[PlayMediaIntentHandler.HandleIntent()] Generating an audio player response");

                // Return audio only response
                response = ResponseBuilder.AudioPlayerPlay(PlayBehavior.ReplaceAll, url, fileName);
            }

            return response;
        }
    }
}
