using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using Amazon.Lambda.Core;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using ASMRDarling.API.Handlers;
using ASMRDarling.API.Interfaces;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ASMRDarling.API
{
    public class Function
    {
        public const string INVOCATION_NAME = "Darling's Gift";

        ILaunchRequestHandler _launchRequestHandler = new LaunchRequestHandler();
        IIntentRequestHandler _intentRequestHandler = new IntentRequestHandler();


        public async Task<SkillResponse> FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            var logger = context.Logger;
            logger.LogLine($"[Function.FunctionHandler()] {INVOCATION_NAME} launched");

            bool hasDisplay = input.Context.System.Device.SupportedInterfaces.ContainsKey("Display");
            logger.LogLine($"[Function.FunctionHandler()] Diplay availability: {hasDisplay}");
            logger.LogLine($"[Function.FunctionHandler()] Input details: {JsonConvert.SerializeObject(input)}");

            var requestType = input.Request.Type;
            logger.LogLine($"[Function.FunctionHandler()] Request type: {requestType}");

            Session session = input.Session;
            if (session.Attributes == null)
                session.Attributes = new Dictionary<string, object>();
            session.Attributes["has_display"] = hasDisplay;
            logger.LogLine($"[Function.FunctionHandler()] Session details: {JsonConvert.SerializeObject(session)}");


            SkillResponse response = new SkillResponse();

            switch (requestType)
            {
                case "LaunchRequest":
                    var launchRequest = input.Request as LaunchRequest;
                    logger.LogLine($"[Function.FunctionHandler()] Launch request processing started");
                    response = await _launchRequestHandler.HandleRequest(launchRequest, logger);
                    break;

                case "IntentRequest":
                    var intentRequest = input.Request as IntentRequest;
                    logger.LogLine($"[Function.FunctionHandler()] Intent request processing started");
                    response = await _intentRequestHandler.HandleRequest(intentRequest, session, logger);
                    break;
            }

            logger.LogLine($"[Function.FunctionHandler()] Response from the API: {JsonConvert.SerializeObject(response.Response)}");

            return response;
        }
    }
}
