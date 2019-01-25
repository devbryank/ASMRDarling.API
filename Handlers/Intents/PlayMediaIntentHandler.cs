using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Alexa.NET.Request;
using Alexa.NET.Response;
using ASMRDarling.API.Interfaces;
using System.Text.RegularExpressions;
using Alexa.NET.Response.Directive;

namespace ASMRDarling.API.Handlers
{
    class PlayMediaIntentHandler : IPlayMediaIntentHandler
    {
        public const string MEDIA_FILE_SLOT_NAME = "MediaFileName";
        public const string MEDIA_BASE_URL = "https://s3.amazonaws.com/asmr-darling-api-media";


        public PlayMediaIntentHandler() { }


        public async Task<SkillResponse> HandleIntent(Intent intent, Session session, ILambdaLogger logger)
        {
            Slot slot = intent.Slots[MEDIA_FILE_SLOT_NAME];
            string slotValue = slot.Value;

            logger.LogLine($"[PlayMediaIntentHandler.HandleIntent()] Requested slot value (synonym): {slotValue}");

            // Handling synonyms (if multiple slots & values are available, this might not work as intended)
            ResolutionAuthority[] resolution = slot.Resolution.Authorities;
            ResolutionValueContainer[] container = resolution[0].Values;

            bool? hasDisplay = session.Attributes["has_display"] as bool?;
            string fileType = hasDisplay == true ? "mp4" : "mp3";
            string fileName = Regex.Replace(container[0].Value.Name, @"\s", "");

            logger.LogLine($"[PlayMediaIntentHandler.HandleIntent()] Media file requested: {fileName}.{fileType}");

#warning make url builder mp3, mp4, image, screen availability should have been figured out by now

            string url = $"{MEDIA_BASE_URL}/{fileType}/{fileName}.{fileType}";

            logger.LogLine($"[PlayMediaIntentHandler.HandleIntent()] Media file source URL: {url}");

            return ResponseBuilder.AudioPlayerPlay(PlayBehavior.ReplaceAll, url, fileName);
        }
    }
}
