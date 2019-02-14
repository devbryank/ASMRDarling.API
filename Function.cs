using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json;
using Amazon.Lambda.Core;

using Alexa.NET.Request;
using Alexa.NET.Response;
using ASMRDarling.API.Data;
using ASMRDarling.API.Handlers;
using ASMRDarling.API.Interfaces;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ASMRDarling.API
{
    /// <summary>
    /// Entry point for the AWS Lambda function
    /// </summary>
    public class Function
    {
        // Request handlers
        readonly ILaunchRequestHandler launchRequestHandler = new LaunchRequestHandler();
        readonly IIntentRequestHandler intentRequestHandler = new IntentRequestHandler();


        // Handle the incoming request from Alexa
        public async Task<SkillResponse> FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            SkillResponse response = new SkillResponse();
            new UserEventRequestHandler().AddToRequestConverter();

            // Start logging
            ILambdaLogger logger = context.Logger;
            logger.LogLine($"[Function.FunctionHandler()] Alexa skill {AlexaConstants.Invocation} launched");
            logger.LogLine($"[Function.FunctionHandler()] Input details: {JsonConvert.SerializeObject(input)}");
            logger.LogLine($"[Function.FunctionHandler()] Lambda context details: {JsonConvert.SerializeObject(context)}");


            // Get request type
            string requestType = input.Request.Type;
            string[] requestTypes = requestType.Split('.');
            string mainRequestType = requestTypes[0];
            string subRequestType = requestTypes[requestTypes.Length - 1];

            logger.LogLine($"[Function.FunctionHandler()] Main request type: {mainRequestType}");

            if (subRequestType != null || !subRequestType.Equals(mainRequestType))
                logger.LogLine($"[Function.FunctionHandler()] Sub request type: {subRequestType} derived from {requestType}");


            // Check if the device has a display interface
            bool hasDisplay = input.Context.System.Device.SupportedInterfaces.ContainsKey("Display");
            logger.LogLine($"[Function.FunctionHandler()] Diplay availability: {hasDisplay}");


            // Initialize & set session attributes
            Session session = input.Session;

            if (session == null || session.Attributes == null)
            {
                logger.LogLine($"[Function.FunctionHandler()] Session state is empty, going to be initialized");

                session = new Session
                {
                    Attributes = new Dictionary<string, object>()
                };
            }

            logger.LogLine($"[Function.FunctionHandler()] Setting up essential session attributes");

            session.Attributes["has_display"] = hasDisplay;
            session.Attributes["quick_response"] = new ProgressiveResponse(input);
            session.Attributes["current_audio_item"] = input.Context.AudioPlayer.Token;

            logger.LogLine($"[Function.FunctionHandler()] Session details: {JsonConvert.SerializeObject(session)}");


            // Direct request into the appropriate handler
            switch (mainRequestType)
            {
                // Handle launch request
                case AlexaConstants.LaunchRequest:
                    logger.LogLine($"[Function.FunctionHandler()] Directing request into {AlexaConstants.LaunchRequest} handler");
                    response = await launchRequestHandler.HandleRequest(input, session, logger);
                    break;

                // Handle intent request
                case AlexaConstants.IntentRequest:
                    logger.LogLine($"[Function.FunctionHandler()] Directing request into {AlexaConstants.IntentRequest} handler");
                    response = await intentRequestHandler.HandleRequest(input, session, logger);
                    break;

                default:
                    break;
            }


            // Return response
            logger.LogLine($"[Function.FunctionHandler()] Response details: {JsonConvert.SerializeObject(response)}");
            return response;
        }
    }
}
