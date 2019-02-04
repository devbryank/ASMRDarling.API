using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using Amazon.Lambda.Core;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Alexa.NET.APL;
using ASMRDarling.API.Handlers;
using ASMRDarling.API.Templates;
using ASMRDarling.API.Interfaces;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ASMRDarling.API
{
    public class Function
    {

#warning intent & request names (constants) can be referred to AlexaConstants (a class contains all of the constants)

        // request names
        const string InvocationName = "Darling's Gift";
        const string LaunchRequestName = "LaunchRequest";
        const string IntentRequestName = "IntentRequest";
        const string AudioPlayerRequestName = "AudioPlayer";
        const string AlexaRequestName = "Alexa";
        const string ExceptionRequestName = "System";
        const string SessionEndedRequestName = "SessionEndedRequest";

        // request handlers
        readonly ILaunchRequestHandler launchRequestHandler = new LaunchRequestHandler();
        readonly IIntentRequestHandler intentRequestHandler = new IntentRequestHandler();
        readonly IAudioPlayerRequestHandler audioPlayerRequestHandler = new AudioPlayerRequestHandler();
        readonly IAlexaRequestHandler alexaRequestHandler = new AlexaRequestHandler();
        readonly ISessionEndedRequestHandler sessionEndedRequestHandler = new SessionEndedRequestHandler();


        public async Task<SkillResponse> FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            // start logging
            var logger = context.Logger;
            logger.LogLine($"[Function.FunctionHandler()] {InvocationName} launched");
            logger.LogLine($"[Function.FunctionHandler()] Input details: {JsonConvert.SerializeObject(input)}");

            // adding user event request converter
            new UserEventRequestHandler().AddToRequestConverter();

            // get request type
            string requestType = input.Request.Type;
            logger.LogLine($"[Function.FunctionHandler()] Request type: {requestType}");

            // check if the device has a display interface
            bool hasDisplay = input.Context.System.Device.SupportedInterfaces.ContainsKey("Display");
            logger.LogLine($"[Function.FunctionHandler()] Diplay availability: {hasDisplay}");


            // initialize and get session states
            if (input.Session == null)
                input.Session = new Session();

            Session session = input.Session;

            if (session.Attributes == null)
                session.Attributes = new Dictionary<string, object>();

            session.Attributes["has_display"] = hasDisplay;
            session.Attributes["quick_response"] = new ProgressiveResponse(input);

            if (input.Context.AudioPlayer.Token != null)
                session.Attributes["current_audio_item"] = input.Context.AudioPlayer.Token;
            else
                session.Attributes["current_audio_item"] = null;

            if (!session.Attributes.ContainsKey("current_video_item"))
                session.Attributes["current_video_item"] = null;

            if (input.Request is UserEventRequest userEvent)
            {
                var token = userEvent.Token;
                var argument = userEvent.Arguments[0]; ;
            }

            logger.LogLine($"[Function.FunctionHandler()] Session details: {JsonConvert.SerializeObject(session)}");


            // declare response to return
            SkillResponse response = new SkillResponse();

            // direct request into the matching handler
            switch (requestType.Split('.')[0])
            {
                // handle launch request
                case LaunchRequestName:
                    logger.LogLine($"[Function.FunctionHandler()] Directing request into {LaunchRequestName} handler");
                    response = await launchRequestHandler.HandleRequest(input, session, logger);
                    break;

                // handle intent request
                case IntentRequestName:
                    logger.LogLine($"[Function.FunctionHandler()] Directing request into {IntentRequestName} handler");
                    response = await intentRequestHandler.HandleRequest(input, session, logger);
                    break;

                // handle audio player request
                case AudioPlayerRequestName:
                    logger.LogLine($"[Function.FunctionHandler()] Directing request into {AudioPlayerRequestName} handler");
                    response = await audioPlayerRequestHandler.HandleRequest(input, session, logger);
                    break;

                // handle alexa request
                case AlexaRequestName:
                    logger.LogLine($"[Function.FunctionHandler()] Directing request into {AlexaRequestName} handler");
                    response = await alexaRequestHandler.HandleRequest(input, session, logger);
                    break;

                // handle system exception request
                case ExceptionRequestName:

#warning not yet implemented

                    logger.LogLine($"[Function.FunctionHandler()] Directing request into {ExceptionRequestName} handler");
                    response = ResponseBuilder.Empty();
                    break;

                // handle session ended request
                case SessionEndedRequestName:
                    logger.LogLine($"[Function.FunctionHandler()] Directing request into {SessionEndedRequestName} handler");
                    response = await sessionEndedRequestHandler.HandleRequest(input, session, logger);
                    break;

                // default fallback case
                default:
                    logger.LogLine($"[Function.FunctionHandler()] Request was not supported, directing request into the default case");
                    var output = SsmlTemplate.ExceptionSpeech();
                    response = ResponseBuilder.Ask(output, null);
                    break;
            }

            // log response details then return
            logger.LogLine($"[Function.FunctionHandler()] Response details: {JsonConvert.SerializeObject(response)}");
            return response;
        }
    }
}
