using System;
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


        // Slot names
        public const string MEDIA_FILE_SLOT_NAME = "MediaFileName";


        //public const string clipURL = "https://s3.amazonaws.com/asmr-darling-api-media/mp3/10TriggersToHelpYouSleep.mp3";


        public async Task<SkillResponse> FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            SkillResponse response = new SkillResponse();

            var log = context.Logger;
            log.LogLine($"<Devlog> {INVOCATION_NAME} skill started");

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

                //response = ResponseBuilder.Ask(output, reprompt);

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
                log.LogLine($"Intent requested: {((IntentRequest)input.Request).Intent.Name}");
                var intentRequest = input.Request as IntentRequest;
                var fileRequested = intentRequest.Intent.Slots[MEDIA_FILE_SLOT_NAME].Value;

                log.LogLine($"File requested: {fileRequested}");

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

                log.LogLine($"Intent name is  { intentRequest.Intent.Name }");

                switch (intentRequest.Intent.Name)
                {
                    case MEDIA_INTENT_NAME:
                        log.LogLine("switch entered");
                        try
                        {
                            log.LogLine("HI THIS IS THE RIGHT PLACE");
                            response = ResponseBuilder.AudioPlayerPlay(PlayBehavior.ReplaceAll, clipURL, "Any Token");
                        }
                        catch (Exception ex)
                        {
                            log.LogLine(ex.ToString());
                        }
                        break;
                    default:
                        var output = new SsmlOutputSpeech()
                        {
                            Ssml = "<speak>" +
                                "<amazon:effect name='whispered'>" +
                                    "<prosody rate='slow'>" +
                                        "I am ending the session." +
                                    "</prosody>" +
                                "</amazon:effect>" +
                           "</speak>"
                        };

                        response = ResponseBuilder.Tell(output);
                        break;
                }


            }
            //else
            //{
            //    return MakeSkillResponse("Sorry I did not get the intent", false);
            //}

            return response;
        }


        //private SkillResponse MakeSkillResponse(string outputSpeech, bool shouldEndSession, string repromptText = "Hello world.")
        //{
        //    var response = new ResponseBody
        //    {
        //        ShouldEndSession = shouldEndSession,
        //        OutputSpeech = new PlainTextOutputSpeech { Text = outputSpeech }
        //    };

        //    var skillResponse = new SkillResponse
        //    {
        //        Response = response,
        //        Version = "1.0"
        //    };

        //    return skillResponse;
        //}
    }
}
