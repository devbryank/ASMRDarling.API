using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Amazon.Lambda.Core;

using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Alexa.NET.Response.Directive;
using System.Text.RegularExpressions;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]


namespace ASMRDarling.API
{
    public class Function
    {
        // Invocation
        public const string INVOCATION_NAME = "Darling's Gift";

        // Intent names
        public const string MEDIA_INTENT_NAME = "PlayMedia";

        // Slot names
        public const string MEDIA_FILE_SLOT_NAME = "MediaFileName";

        // Base URL
        public const string MEDIA_BASE_URL = "https://s3.amazonaws.com/asmr-darling-api-media";


        // what is your name?? ask and store dynamo db?

        // tell list of clips that she can play (another intent?).


        public async Task<SkillResponse> FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            SkillResponse response = new SkillResponse();

            var log = context.Logger;
            var requestType = input.GetRequestType();

            log.LogLine($"<Devlog> {INVOCATION_NAME} started");
            log.LogLine($"<Devlog> Input info: {JsonConvert.SerializeObject(input)}");
            log.LogLine($"<Devlog> Request type: {requestType}");

            if (requestType == typeof(LaunchRequest))
            {
                log.LogLine($"<Devlog> Default launch request processing started");

                var output = new SsmlOutputSpeech()
                {
                    Ssml = "<speak>" +
                                "<amazon:effect name='whispered'>" +
                                    "<prosody rate='slow'>" +
                                        "Hey, it's me. ASMR Darling." +
                                        "You can say things like, play 10 triggers to help you sleep, to begin." +
                                    "</prosody>" +
                                "</amazon:effect>" +
                           "</speak>"
                };

                response = ResponseBuilder.Ask(output, null);
            }
            else if (requestType == typeof(IntentRequest))
            {
                var intentRequest = input.Request as IntentRequest;

                log.LogLine($"<Devlog> Intent requested: {intentRequest.Intent.Name}");

                switch (intentRequest.Intent.Name)
                {
                    case MEDIA_INTENT_NAME:
                        try
                        {
                            Slot slot = intentRequest.Intent.Slots[MEDIA_FILE_SLOT_NAME];
                            string slotValue = slot.Value;

                            log.LogLine($"<Devlog> Requested slot value (synonym): {slotValue}");

                            // Handling synonyms (if multiple slots & values are available, this might not work as intended)
                            ResolutionAuthority[] resolution = slot.Resolution.Authorities;
                            ResolutionValueContainer[] container = resolution[0].Values;

                            string fileType = "mp3";
                            string fileName = Regex.Replace(container[0].Value.Name, @"\s", "");

                            log.LogLine($"<Devlog> Media file requested: {fileName}.{fileType}");

#warning make url builder mp3, mp4, image, screen availability should have been figured out by now

                            string url = $"{MEDIA_BASE_URL}/{fileType}/{fileName}.{fileType}";

                            log.LogLine($"<Devlog> Media file source URL: {url}");

                            response = ResponseBuilder.AudioPlayerPlay(PlayBehavior.ReplaceAll, url, fileName);
                        }
                        catch (Exception ex)
                        {
                            log.LogLine($"<Devlog> Exception caught: {ex.ToString()}");
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

            log.LogLine($"<Devlog> Response from the API {response}");

            return response;
        }
    }
}
