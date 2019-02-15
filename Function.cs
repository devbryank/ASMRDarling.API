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
        // request handlers
        ILaunchRequestHandler launchRequestHandler;
        IIntentRequestHandler intentRequestHandler;
        //readonly IAudioPlayerRequestHandler audioPlayerRequestHandler = new AudioPlayerRequestHandler();
        //readonly IAlexaRequestHandler alexaRequestHandler = new AlexaRequestHandler();
        //readonly ISessionEndedRequestHandler sessionEndedRequestHandler = new SessionEndedRequestHandler();


        // handle request from alexa
        public async Task<SkillResponse> FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            SkillResponse response = new SkillResponse();
            new UserEventRequestHandler().AddToRequestConverter();


            // start logging
            ILambdaLogger logger = context.Logger;
            logger.LogLine($"[Function.FunctionHandler()] Alexa skill {AlexaConstants.Invocation} launched");
            logger.LogLine($"[Function.FunctionHandler()] Input details: {JsonConvert.SerializeObject(input)}");
            logger.LogLine($"[Function.FunctionHandler()] Lambda context details: {JsonConvert.SerializeObject(context)}");


            // get main & sub request types
            string requestType = input.Request.Type;
            string[] requestTypes = requestType.Split('.');
            string mainRequestType = requestTypes[0];
            string subRequestType = requestTypes.Length > 1 ? requestTypes[requestTypes.Length - 1] : null;

            logger.LogLine($"[Function.FunctionHandler()] Main request type: {mainRequestType}");

            if (subRequestType != null)
                logger.LogLine($"[Function.FunctionHandler()] Sub request type: {subRequestType} derived from {requestType}");


            // check if the device has a display interface
            bool hasDisplay = input.Context.System.Device.SupportedInterfaces.ContainsKey("Display");
            logger.LogLine($"[Function.FunctionHandler()] Diplay availability: {hasDisplay}");


            // initialize & load db components for states
            logger.LogLine($"[Function.FunctionHandler()] Acquiring user state from DynamoDB");

            MediaStateHelper mediaStateHelper = new MediaStateHelper(logger);
            await mediaStateHelper.VerifyTable();
            string userId = input.Session != null ? input.Session.User.UserId : input.Context.System.User.UserId;

            var lastState = await mediaStateHelper.GetMediaState(userId); // this will return an initial default state if no previous states exist
            var currentState = new MediaState() { UserId = userId };

            currentState.State = lastState.State;

            logger.LogLine($"[Function.FunctionHandler()] Current user state: {JsonConvert.SerializeObject(currentState)}");


            // initialize & set session attributes
            Session session = input.Session;
            if (session.Attributes == null)
            {
                logger.LogLine($"[Function.FunctionHandler()] New session started, initializing attributes");
                session.Attributes = new Dictionary<string, object>();
            }

            logger.LogLine($"[Function.FunctionHandler()] Setting up essential session attributes");

            session.Attributes["has_display"] = hasDisplay;
            session.Attributes["quick_response"] = new ProgressiveResponse(input);

            logger.LogLine($"[Function.FunctionHandler()] Session details: {JsonConvert.SerializeObject(session)}");



            //session.Attributes["current_audio_item"] = input.Context.AudioPlayer.Token;
            //if (!session.Attributes.ContainsKey("user_state"))
            //    session.Attributes["user_state"] = string.Empty;



            // direct request into the appropriate handler
            switch (mainRequestType)
            {
                // handle launch request
                case AlexaConstants.LaunchRequest:
                    launchRequestHandler = new LaunchRequestHandler();
                    logger.LogLine($"[Function.FunctionHandler()] Directing request into {AlexaConstants.LaunchRequest} handler");
                    response = await launchRequestHandler.HandleRequest(input, currentState, session, logger);
                    await mediaStateHelper.SaveMediaState(currentState);
                    break;


                // handle intent request
                case AlexaConstants.IntentRequest:
                    intentRequestHandler = new IntentRequestHandler();
                    logger.LogLine($"[Function.FunctionHandler()] Directing request into {AlexaConstants.IntentRequest} handler");
                    response = await intentRequestHandler.HandleRequest(input, currentState, session, logger);
                    break;


                //                // handle audio player request
                //                case AudioPlayerRequestName:
                //                    logger.LogLine($"[Function.FunctionHandler()] Directing request into {AudioPlayerRequestName} handler");
                //                    response = await audioPlayerRequestHandler.HandleRequest(input, session, logger);
                //                    break;

                //                // handle alexa request
                //                case AlexaRequestName:
                //                    logger.LogLine($"[Function.FunctionHandler()] Directing request into {AlexaRequestName} handler");
                //                    response = await alexaRequestHandler.HandleRequest(input, session, logger);
                //                    break;

                //                // handle system exception request
                //                case ExceptionRequestName:

                //#warning not yet implemented

                //                    logger.LogLine($"[Function.FunctionHandler()] Directing request into {ExceptionRequestName} handler");
                //                    response = ResponseBuilder.Empty();
                //                    break;

                //                // handle session ended request
                //                case SessionEndedRequestName:
                //                    logger.LogLine($"[Function.FunctionHandler()] Directing request into {SessionEndedRequestName} handler");
                //                    response = await sessionEndedRequestHandler.HandleRequest(input, session, logger);
                //                    break;


                // handle default fallback case
                default:
                    logger.LogLine($"[Function.FunctionHandler()] Request was not recognized, directing request into the default case handler");
                    var output = SsmlTemplate.RequestFallbackSpeech();
                    response = ResponseBuilder.Ask(output, null);
                    break;
            }

            // return response back to alexa
            logger.LogLine($"[Function.FunctionHandler()] Response details: {JsonConvert.SerializeObject(response)}");
            return response;
        }
    }
}
