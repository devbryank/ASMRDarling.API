using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using System.Threading.Tasks;
using ASMRDarling.API.Templates;
using ASMRDarling.API.Interfaces;

namespace ASMRDarling.API.Handlers
{
    class LaunchRequestHandler : ILaunchRequestHandler
    {
        // Constructor
        public LaunchRequestHandler() { }

        // Request handler
        public async Task<SkillResponse> HandleRequest(SkillRequest input, Session session, ILambdaLogger logger)
        {
            logger.LogLine($"[LaunchRequestHandler.HandleRequest()] Launch request handling started");

            // Get launch request
            LaunchRequest launchRequest = input.Request as LaunchRequest;

            // Get session display value, then make output speech
            bool? hasDisplay = session.Attributes["has_display"] as bool?;
            SsmlOutputSpeech output = SsmlTemplate.LaunchSpeech(hasDisplay);

            if (hasDisplay == true)
            {
                // If the device has display
                logger.LogLine($"[LaunchRequestHandler.HandleRequest()] Generating initial APL response");

                // Return APL response
                var response = ResponseBuilder.Ask(output, null);
                return await AplTemplate.MenuDisplay(response);
            }
            else
            {
                // If display is not available
                logger.LogLine($"[LaunchRequestHandler.HandleRequest()] Generating initial audio only response");

                // Return audio only response
                return ResponseBuilder.Ask(output, null);
            }
        }
    }
}
