using Newtonsoft.Json;
using Amazon.Lambda.Core;
using System.Threading.Tasks;
using System.Collections.Generic;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;
using ASMRDarling.API.Models;
using ASMRDarling.API.Helpers;
using ASMRDarling.API.Handlers;
using ASMRDarling.API.Templates;
using ASMRDarling.API.Interfaces;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ASMRDarling.API
{
    /// <summary>
    /// entry point of the lambda function
    /// </summary>
    public class Function
    {
        // handle request from alexa
        public async Task<SkillResponse> FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            SkillResponse response = new SkillResponse();
            new UserEventRequestHandler().AddToRequestConverter();
            //new SystemExceptionEncounteredRequestTypeConverter().AddToRequestConverter();


            // start logging
            ILambdaLogger logger = context.Logger;
            logger.LogLine($"[Function.FunctionHandler()] Alexa skill <{AlexaRequestConstants.Invocation}> launched");
            logger.LogLine($"[Function.FunctionHandler()] Input details: {JsonConvert.SerializeObject(input)}");
            logger.LogLine($"[Function.FunctionHandler()] Lambda context details: {JsonConvert.SerializeObject(context)}");


            // get main & sub request types
            string requestType = input.Request.Type;
            string[] requestTypes = requestType.Split('.');
            string mainRequestType = requestTypes[0];
            string subRequestType = requestTypes.Length > 1 ? requestTypes[requestTypes.Length - 1] : null;

            logger.LogLine($"[Function.FunctionHandler()] Main request type: <{mainRequestType}>");
            if (subRequestType != null)
                logger.LogLine($"[Function.FunctionHandler()] Sub request type: <{subRequestType}> derived from {requestType}");


            // check if the device has a display interface
            bool hasDisplay = input.Context.System.Device.SupportedInterfaces.ContainsKey("Display");
            logger.LogLine($"[Function.FunctionHandler()] Diplay availability: {hasDisplay}");


            // initialize & load db components for managing user state
            logger.LogLine($"[Function.FunctionHandler()] Acquiring user's media information from database");

            MediaStateHelper mediaStateHelper = new MediaStateHelper(logger);
            await mediaStateHelper.VerifyTable();

            string userId = input.Session != null ? input.Session.User.UserId : input.Context.System.User.UserId;
            var lastState = await mediaStateHelper.GetMediaState(userId);
            var currentState = new MediaState() { UserId = userId };
            currentState.State = lastState.State;

            logger.LogLine($"[Function.FunctionHandler()] Current user state: {JsonConvert.SerializeObject(currentState)}");


            // initialize & set session attributes
            Session session = input.Session;

            if (session == null || session.Attributes == null)
            {
                logger.LogLine($"[Function.FunctionHandler()] New session started, initializing session & attributes");
                session = new Session { Attributes = new Dictionary<string, object>() };
            }

            logger.LogLine($"[Function.FunctionHandler()] Setting up essential session attributes");

            session.Attributes["sub_intent"] = string.Empty;
            session.Attributes["has_display"] = hasDisplay;
            session.Attributes["quick_response"] = new ProgressiveResponse(input);

            logger.LogLine($"[Function.FunctionHandler()] Session details: {JsonConvert.SerializeObject(session)}");


            // direct request into an appropriate handler
            switch (mainRequestType)
            {
                // handle launch request
                case AlexaRequestConstants.LaunchRequest:
                    ILaunchRequestHandler launchRequestHandler = new LaunchRequestHandler();
                    logger.LogLine($"[Function.FunctionHandler()] Directing request into {AlexaRequestConstants.LaunchRequest} handler");
                    response = await launchRequestHandler.HandleRequest(input, currentState, session, logger);
                    break;


                // handle intent request
                case AlexaRequestConstants.IntentRequest:
                    IIntentRequestHandler intentRequestHandler = new IntentRequestHandler();
                    logger.LogLine($"[Function.FunctionHandler()] Directing request into {AlexaRequestConstants.IntentRequest} handler");
                    response = await intentRequestHandler.HandleRequest(input, currentState, session, logger);
                    break;


                // handle audio player request
                case AlexaRequestConstants.AudioPlayer:
                    IAudioPlayerRequestHandler audioPlayerRequestHandler = new AudioPlayerRequestHandler();
                    logger.LogLine($"[Function.FunctionHandler()] Directing request into {AlexaRequestConstants.AudioPlayer} handler");
                    response = await audioPlayerRequestHandler.HandleRequest(input, currentState, session, logger);
                    break;


                // handle alexa user event request
                case AlexaRequestConstants.Alexa:
                    IAlexaRequestHandler alexaRequestHandler = new AlexaRequestHandler();
                    logger.LogLine($"[Function.FunctionHandler()] Directing request into {AlexaRequestConstants.Alexa} handler");
                    response = await alexaRequestHandler.HandleRequest(input, currentState, session, logger);
                    break;


                // handle session ended request
                case AlexaRequestConstants.SessionEndedRequest:
                    ISessionEndedRequestHandler sessionEndedRequestHandler = new SessionEndedRequestHandler();
                    logger.LogLine($"[Function.FunctionHandler()] Directing request into {AlexaRequestConstants.SessionEndedRequest} handler");
                    response = await sessionEndedRequestHandler.HandleRequest(input, currentState, session, logger);
                    break;

#warning request converter should be implemented first to direct the request into this case
                // handle system exception request
                case AlexaRequestConstants.System:
                    logger.LogLine($"[Function.FunctionHandler()] Directing request into {AlexaRequestConstants.System} handler");
                    response = ResponseBuilder.Empty();
                    break;


                // handle default fallback case
                default:
                    logger.LogLine($"[Function.FunctionHandler()] Request was not recognized, directing request into the default case handler");
                    var output = SsmlTemplate.RequestFallbackSpeech();
                    response = ResponseBuilder.Ask(output, null);
                    break;
            }


            // save state & return response back to alexa
            logger.LogLine($"[Function.FunctionHandler()] Updated user's media state details: {JsonConvert.SerializeObject(currentState)}");
            logger.LogLine($"[Function.FunctionHandler()] Response details: {JsonConvert.SerializeObject(response)}");

            await mediaStateHelper.SaveMediaState(currentState);
            return response;
        }
    }
}
