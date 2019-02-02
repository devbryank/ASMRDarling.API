using Newtonsoft.Json;
using Amazon.Lambda.Core;
using System.Threading.Tasks;
using System.Collections.Generic;
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
#warning https://github.com/matthiasxc/alexa-audio-tutorial/blob/master/AudioSkillSample/Assets/Constants.cs
#warning implement try & catch clause to log exceptions

        // Request names
        const string InvocationName = "Darling's Gift";
        const string LaunchRequestName = "LaunchRequest";
        const string IntentRequestName = "IntentRequest";
        const string AlexaRequestName = "Alexa.Presentation.APL.UserEvent";
        const string SessionEndedRequestName = "SessionEndedRequest";

        // Request handlers
        readonly UserEventRequestHandler userRequestHandler = new UserEventRequestHandler();
        readonly ILaunchRequestHandler launchRequestHandler = new LaunchRequestHandler();
        readonly IIntentRequestHandler intentRequestHandler = new IntentRequestHandler();
        readonly IAlexaRequestHandler alexaRequestHandler = new AlexaRequestHandler();
        readonly ISessionEndedRequestHandler sessionEndedRequestHandler = new SessionEndedRequestHandler();


        // Function handler
        public async Task<SkillResponse> FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            // Start logging
            var logger = context.Logger;
            logger.LogLine($"[Function.FunctionHandler()] {InvocationName} launched");
            logger.LogLine($"[Function.FunctionHandler()] Input details: {JsonConvert.SerializeObject(input)}");

            // Adding user event request handler
            userRequestHandler.AddToRequestConverter();

            // Get request type
            string requestType = input.Request.Type;
            logger.LogLine($"[Function.FunctionHandler()] Request type: {requestType}");

            // Check if the device has a display interface
            bool hasDisplay = input.Context.System.Device.SupportedInterfaces.ContainsKey("Display");
            logger.LogLine($"[Function.FunctionHandler()] Diplay availability: {hasDisplay}");

            // Store session state
            Session session = input.Session;

            if (session.Attributes == null)
                session.Attributes = new Dictionary<string, object>();

            session.Attributes["has_display"] = hasDisplay;
            session.Attributes["current_clip"] = input.Context.AudioPlayer.Token;

            if (input.Request is UserEventRequest userEvent)
            {
                var token = userEvent.Token;
                var argument = userEvent.Arguments[0];

#warning session attribute argument is not used

                session.Attributes["argument"] = argument;
            }

            logger.LogLine($"[Function.FunctionHandler()] Session details: {JsonConvert.SerializeObject(session)}");

            // Declare response to return
            SkillResponse response = new SkillResponse();

            // Direct request into the matching handler
            switch (requestType)
            {
                // Handle launch request
                case LaunchRequestName:
                    logger.LogLine($"[Function.FunctionHandler()] Directing request into {LaunchRequestName} handler");
                    response = await launchRequestHandler.HandleRequest(input, session, logger);
                    break;

                // Handle intent request
                case IntentRequestName:
                    logger.LogLine($"[Function.FunctionHandler()] Directing request into {IntentRequestName} handler");
                    response = await intentRequestHandler.HandleRequest(input, session, logger);
                    break;

                // Handle alexa request
                case AlexaRequestName:
                    logger.LogLine($"[Function.FunctionHandler()] Directing request into {AlexaRequestName} handler");
                    response = await alexaRequestHandler.HandleRequest(input, session, logger);
                    break;

                // Handle session ended request
                case SessionEndedRequestName:
                    logger.LogLine($"[Function.FunctionHandler()] Directing request into {SessionEndedRequestName} handler");
                    response = await sessionEndedRequestHandler.HandleRequest(input, session, logger);
                    break;

                // Default fallback case
                default:
                    logger.LogLine($"[Function.FunctionHandler()] Request was not supported, directing request into the default case");
                    var output = SsmlTemplate.ExceptionSpeech();
                    response = ResponseBuilder.Ask(output, null);
                    break;
            }

            // Log response details
            logger.LogLine($"[Function.FunctionHandler()] Response details: {JsonConvert.SerializeObject(response.Response)}");

            return response;
        }
    }
}
