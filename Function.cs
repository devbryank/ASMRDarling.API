using System;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Amazon.Lambda.Core;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Alexa.NET.Response.Directive;
using ASMRDarling.API.Interfaces;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ASMRDarling.API
{
    public class Function
    {
        public const string INVOCATION_NAME = "Darling's Gift";
        public const string MEDIA_INTENT_NAME = "PlayMedia";
        public const string MEDIA_FILE_SLOT_NAME = "MediaFileName";
        public const string MEDIA_BASE_URL = "https://s3.amazonaws.com/asmr-darling-api-media";

        private readonly ILaunchRequestHandler _launchRequestHandler;


        public async Task<SkillResponse> FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            var log = context.Logger;
            log.LogLine($"[ASMRDaring.API] {INVOCATION_NAME} launched");
            log.LogLine($"[ASMRDaring.API] Input details: {JsonConvert.SerializeObject(input)}");

            var requestType = input.GetRequestType().ToString();
            log.LogLine($"[ASMRDaring.API] Request type: {requestType}");

            SkillResponse response = new SkillResponse();

            switch (requestType)
            {
                case "LaunchRequest":
                    log.LogLine($"[ASMRDaring.API] Launch request processing started");
                    response = await _launchRequestHandler.HandleRequest(input.Request as LaunchRequest);
                    break;
                case "IntentRequest":
                    log.LogLine($"[ASMRDaring.API] Intent request processing started");

                    break;
            }






            if (requestType == typeof(IntentRequest))
            {
                var intentRequest = input.Request as IntentRequest;

                log.LogLine($"[ASMRDaring.API] Intent requested: {intentRequest.Intent.Name}");

                switch (intentRequest.Intent.Name)
                {
                    case MEDIA_INTENT_NAME:
                        try
                        {
                            Slot slot = intentRequest.Intent.Slots[MEDIA_FILE_SLOT_NAME];
                            string slotValue = slot.Value;

                            log.LogLine($"[ASMRDaring.API] Requested slot value (synonym): {slotValue}");

                            // Handling synonyms (if multiple slots & values are available, this might not work as intended)
                            ResolutionAuthority[] resolution = slot.Resolution.Authorities;
                            ResolutionValueContainer[] container = resolution[0].Values;

                            bool hasDisplay = input.Context.System.Device.SupportedInterfaces.ContainsKey("Display");

                            log.LogLine($"[ASMRDaring.API] Diplay availability: {hasDisplay}");

                            string fileType = hasDisplay == true ? "mp4" : "mp3";
                            string fileName = Regex.Replace(container[0].Value.Name, @"\s", "");

                            log.LogLine($"[ASMRDaring.API] Media file requested: {fileName}.{fileType}");

#warning make url builder mp3, mp4, image, screen availability should have been figured out by now

                            string url = $"{MEDIA_BASE_URL}/{fileType}/{fileName}.{fileType}";

                            log.LogLine($"[ASMRDaring.API] Media file source URL: {url}");

                            response = ResponseBuilder.AudioPlayerPlay(PlayBehavior.ReplaceAll, url, fileName);
                        }
                        catch (Exception ex)
                        {
                            log.LogLine($"[ASMRDaring.API] Exception caught: {ex.ToString()}");
                        }

                        break;
                    default:
                        var output = new SsmlOutputSpeech()
                        {
                            Ssml = "<speak>" +
                                        "<amazon:effect name='whispered'>" +
                                            "<prosody rate='slow'>" +
                                                "Sorry, I didn't get your intention, can you please tell me one more time?." +
                                            "</prosody>" +
                                        "</amazon:effect>" +
                                   "</speak>"
                        };

                        response = ResponseBuilder.Ask(output, null);

                        break;
                }
            }

            log.LogLine($"[ASMRDaring.API] Response from the API {JsonConvert.SerializeObject(response.Response)}");

            return response;
        }
    }
}
