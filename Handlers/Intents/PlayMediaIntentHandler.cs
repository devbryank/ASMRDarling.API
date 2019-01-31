using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Amazon.Lambda.Core;
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
        public const string MediaFileSlotName = "MediaFileName";
        public const string MediaBaseUrl = "https://s3.amazonaws.com/asmr-darling-api-media";


        public PlayMediaIntentHandler() { }


        public async Task<SkillResponse> HandleIntent(Intent intent, Session session, ILambdaLogger logger)
        {
            Slot slot = intent.Slots[MediaFileSlotName];
            string slotValue = slot.Value;

            logger.LogLine($"[PlayMediaIntentHandler.HandleIntent()] Requested slot value (synonym): {slotValue}");

            ResolutionAuthority[] resolution = slot.Resolution.Authorities;
            ResolutionValueContainer[] container = resolution[0].Values;

            bool? hasDisplay = session.Attributes["has_display"] as bool?;
            string fileType = hasDisplay == true ? "mp4" : "mp3";
            var fileName = Regex.Replace(container[0].Value.Name, @"\s", "");

            logger.LogLine($"[PlayMediaIntentHandler.HandleIntent()] Media file requested: {fileName}.{fileType}");

            string url = UrlBuilder.GetS3FileSourceUrl(MediaBaseUrl, fileName, fileType);

            logger.LogLine($"[PlayMediaIntentHandler.HandleIntent()] Media file source URL: {url}");

            SkillResponse response = new SkillResponse();

            //if (hasDisplay == true)
            //{
            //    logger.LogLine($"[PlayMediaIntentHandler.HandleIntent()] Generating a Video App response");

            //    response = ResponseBuilder.Empty();
            //    response.Response.Directives.Add(new VideoAppDirective(url));
            //}
            //else
            //{
            logger.LogLine($"[PlayMediaIntentHandler.HandleIntent()] Generating an Audio Player response");

            response = ResponseBuilder.AudioPlayerPlay(PlayBehavior.ReplaceAll, url, fileName);
            //}

            return response;
        }
    }
}
