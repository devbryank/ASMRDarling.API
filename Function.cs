using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Alexa.NET.APL;
using Amazon.Lambda.Core;
using ASMRDarling.API.Handlers;
using ASMRDarling.API.Interfaces;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ASMRDarling.API
{
    public class Function
    {
        const string InvocationName = "Darling's Gift";
        const string LaunchRequestName = "LaunchRequest";
        const string IntentRequestName = "IntentRequest";

        readonly ILaunchRequestHandler launchRequestHandler = new LaunchRequestHandler();

        UserEventRequestHandler userHandler = new UserEventRequestHandler();


        public async Task<SkillResponse> FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            // Start logging
            var logger = context.Logger;
            logger.LogLine($"[Function.FunctionHandler()] {InvocationName} launched");
            logger.LogLine($"[Function.FunctionHandler()] Input details: {JsonConvert.SerializeObject(input)}");


            // Adding user event request handler
            userHandler.AddToRequestConverter();


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

            if (input.Request is UserEventRequest userEvent)
            {
                var token = userEvent.Token;
                var argument = userEvent.Arguments[0];
                session.Attributes["argument"] = argument;
            }
 
            logger.LogLine($"[Function.FunctionHandler()] Session details: {JsonConvert.SerializeObject(session)}");


            // Declare response to return
            SkillResponse response = new SkillResponse();


            // Direct request into a matching handler
            switch (requestType)
            {
                // Handle launch request
                case LaunchRequestName:
                    logger.LogLine($"[Function.FunctionHandler()] Directing request into {LaunchRequestName} handler");
                    response = await launchRequestHandler.HandleRequest(input, session, logger);
                    break;












                // Handle intent request
                case IntentRequestName:
                    var intentRequest = input.Request as IntentRequest;
                    logger.LogLine($"[Function.FunctionHandler()] Case: {IntentRequestName} processing started");
                    response = await _intentRequestHandler.HandleRequest(intentRequest, session, logger);
                    break;







                case "Alexa.Presentation.APL.UserEvent":
                    logger.LogLine($"[Function.FunctionHandler()] User event request processing started");

                    break;
            }

            // Log response details then return
            logger.LogLine($"[Function.FunctionHandler()] Response from the API: {JsonConvert.SerializeObject(response.Response)}");

            return response;
        }
    }
}
