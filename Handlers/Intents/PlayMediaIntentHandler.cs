using System;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Amazon.Lambda.Core;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Alexa.NET.Response.Directive;
using ASMRDarling.API.Builders;
using ASMRDarling.API.Templates;
using ASMRDarling.API.Interfaces;

namespace ASMRDarling.API.Handlers
{
    class PlayMediaIntentHandler : IPlayMediaIntentHandler
    {
        // source related constants
        public const string MediaFileSlotName = "MediaFileName";
        public const string MediaBaseUrl = "https://s3.amazonaws.com/asmr-darling-api-media";


        public PlayMediaIntentHandler() { }

        public async Task<SkillResponse> HandleIntent(Intent intent, Session session, ILambdaLogger logger)
        {
            try
            {
                logger.LogLine($"[PlayMediaIntentHandler.HandleIntent()] Play Media Intent handling started");

#warning resolution should support multiple slots

                // get slot values
                Slot slot = intent.Slots[MediaFileSlotName];
                string slotValue = slot.Value;
                logger.LogLine($"[PlayMediaIntentHandler.HandleIntent()] Requested slot value (synonym): {slotValue}");

                // get resolution, multiple slots in a dialog will cause an exception
                ResolutionAuthority[] resolution = slot.Resolution.Authorities;
                ResolutionValueContainer[] container = resolution[0].Values;
                string title = container[0].Value.Name;

                // get file extentions based on the display availability
                bool? hasDisplay = session.Attributes["has_display"] as bool?;
                string fileType = hasDisplay == true ? "mp4" : "mp3";

                // store session state for a title
                session.Attributes["current_video_item"] = title;
                logger.LogLine($"[PlayMediaIntentHandler.HandleIntent()] Session state for the current video item: {title}");

                // convert file name into lower cases with no white spaces
                var fileName = Regex.Replace(container[0].Value.Name, @"\s", "").ToLower();
                logger.LogLine($"[PlayMediaIntentHandler.HandleIntent()] Media file requested: {fileName}.{fileType}");

                // get file source url
                string url = UrlBuilder.GetS3FileSourceUrl(MediaBaseUrl, fileName, fileType);
                logger.LogLine($"[PlayMediaIntentHandler.HandleIntent()] Media file source URL: {url}");

                // declare response to return
                SkillResponse response = new SkillResponse();

                // send a quick response before loading the content
                ProgressiveResponse progressiveResponse = session.Attributes["quick_response"] as ProgressiveResponse;
                await progressiveResponse.SendSpeech($"Playing {container[0].Value.Name}");

                if (hasDisplay == true)
                {
                    // if the device has display
                    logger.LogLine($"[PlayMediaIntentHandler.HandleIntent()] Generating a video app or APL video player response");

#warning video app or apl video? which one is better?

                    // set video app response
                    // response = ResponseBuilder.Empty();
                    // response.Response.Directives.Add(new VideoAppDirective(url));

                    // set apl video player response
                    response = ResponseBuilder.Empty();
                    response.Response.ShouldEndSession = false;
                    return await AplTemplate.VideoPlayer(response, url);
                }
                else
                {
                    // if display is not available
                    logger.LogLine($"[PlayMediaIntentHandler.HandleIntent()] Generating an audio player response");

                    // set audio only response
                    response = ResponseBuilder.AudioPlayerPlay(PlayBehavior.ReplaceAll, url, fileName);
                }

                // return response to the intent request handler
                return response;
            }
            catch (Exception ex)
            {
                logger.LogLine($"[PlayMediaIntentHandler.HandleIntent()] Exception caught");
                logger.LogLine($"[PlayMediaIntentHandler.HandleIntent()] Exception log: {ex}");

                // return system exception to the intent request handler
                var output = SsmlTemplate.SystemFaultSpeech();
                return ResponseBuilder.Tell(output);
            }
        }
    }
}
