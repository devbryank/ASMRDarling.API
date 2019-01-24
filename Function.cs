using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Alexa.NET.Response.Directive;
using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ASMRDarling.API
{
    public class Function
    {
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>


        public const string INVOCATION_NAME = "Darling's Gift";

        // Intents
        public const string MEDIA_INTENT_NAME = "PlayMedia";


        // Slot
        public const string MEDIA_FILE_SLOT_NAME = "ASMR_MEDIA_FILE";


        public const string clipURL = "https://s3.amazonaws.com/asmr-darling-api-media/mp3/10TriggersToHelpYouSleep.mp3";


        public async Task<SkillResponse> FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            SkillResponse response = new SkillResponse();

            var log = context.Logger;
            log.LogLine($"{INVOCATION_NAME} skill started");

            var requestType = input.GetRequestType();
            log.LogLine($"Request type: {requestType}");

            if (requestType == typeof(LaunchRequest))
            {
                log.LogLine($"Launch request processing started");

                var output = new SsmlOutputSpeech()
                {
                    Ssml = "<speak>" +
                                "<amazon:effect name='whispered'>" +
                                    "<prosody rate='slow'>" +
                                        "Hey, it's me. ASMR Darling." +
                                    "</prosody>" +
                                "</amazon:effect>" +
                           "</speak>"
                };

                var reprompt = new Reprompt()
                {
                    OutputSpeech = new SsmlOutputSpeech()
                    {
                        Ssml = "<speak>" +
                                    "<amazon:effect name='whispered'>" +
                                        "<prosody rate='slow'>" +
                                            "You can say things like, play 10 triggers help you sleep, to begin." +
                                        "</prosody>" +
                                    "</amazon:effect>" +
                               "</speak>"
                    }
                };

                response = ResponseBuilder.Ask(output, reprompt);

                //var response = new ResponseBody
                //{
                //    ShouldEndSession = false,
                //    OutputSpeech = new SsmlOutputSpeech
                //    {
                //        Ssml = "<speak><audio src='https://s3.amazonaws.com/asmr-darling-api-media/mp3/10TriggersToHelpYouSleep.mp3' /> <amazon:effect name='whispered'> <prosody rate='slow'> Hey, <break time='1s'/> it's me. ASMR Darling. </prosody> </amazon:effect> </speak>"
                //    }

                //    //
                //};

                //var skillResponse = new SkillResponse
                //{
                //    Response = response,
                //    Version = "1.0"
                //};

                //return skillResponse;
            }


            // what is your name?? ask and store dynamo db?

            // tell list of clips that she can play.




            else if (requestType == typeof(IntentRequest))
            {
                var intentRequest = input.Request as IntentRequest;
                var fileRequested = intentRequest?.Intent?.Slots[MEDIA_FILE_SLOT_NAME].Value;

                if (fileRequested == null)
                {
                    //context.Logger.LogLine($"The file {fileRequested} was not avaliable."); // null value anyways
                    var output = new SsmlOutputSpeech()
                    {
                        Ssml = "<speak>" +
                                "<amazon:effect name='whispered'>" +
                                    "<prosody rate='slow'>" +
                                        "Sorry, you need to tell me which file to play." +
                                    "</prosody>" +
                                "</amazon:effect>" +
                           "</speak>"
                    };

                    var reprompt = new Reprompt()
                    {
                        OutputSpeech = new SsmlOutputSpeech()
                        {
                            Ssml = "<speak>" +
                                        "<amazon:effect name='whispered'>" +
                                            "<prosody rate='slow'>" +
                                                "So, which file do you want me to play?" +
                                            "</prosody>" +
                                        "</amazon:effect>" +
                                   "</speak>"
                        }
                    };

                    response = ResponseBuilder.Ask(output, reprompt);
                }

                switch (intentRequest.Intent.Name)
                {
                    case MEDIA_INTENT_NAME:
                        response = ResponseBuilder.AudioPlayerPlay(PlayBehavior.ReplaceAll, clipURL, "Any Token");
                        break;
                }


            }
            //else
            //{
            //    return MakeSkillResponse("Sorry I did not get the intent", false);
            //}

            return response;
        }


        private SkillResponse MakeSkillResponse(string outputSpeech, bool shouldEndSession, string repromptText = "Hello world.")
        {
            var response = new ResponseBody
            {
                ShouldEndSession = shouldEndSession,
                OutputSpeech = new PlainTextOutputSpeech { Text = outputSpeech }
            };

            var skillResponse = new SkillResponse
            {
                Response = response,
                Version = "1.0"
            };

            return skillResponse;
        }
    }
}
