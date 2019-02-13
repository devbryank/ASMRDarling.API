using System.Threading.Tasks;

using Amazon.Lambda.Core;

using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using ASMRDarling.API.Data;
using ASMRDarling.API.Helpers;
using ASMRDarling.API.Interfaces;

namespace ASMRDarling.API.Handlers
{
    /// <summary>
    /// This class processes the launch request
    /// </summary>
    class LaunchRequestHandler : ILaunchRequestHandler
    {
        public Task<SkillResponse> HandleRequest(SkillRequest input, Session session, ILambdaLogger logger)
        {
            return RequestProcessor.ProcessAlexaRequest("LaunchRequestHandler.HandleRequest()", "Launch Request", async () =>
            {
                // Get launch reqeust
                LaunchRequest launchRequest = input.Request as LaunchRequest;


                // Get session display value, then set an output speech
                bool? hasDisplay = session.Attributes["has_display"] as bool?;
                SsmlOutputSpeech output = SsmlTemplate.LaunchSpeech(hasDisplay);

                if (hasDisplay == true)
                {
                    // If the device has a display interface
                    logger.LogLine($"[LaunchRequestHandler.HandleRequest()] Generating initial APL response");

                    // Get APL response then return
                    SkillResponse response = ResponseBuilder.Ask(output, null);
                    return await AplTemplate.MenuDisplay(response);
                }
                else
                {
                    logger.LogLine($"[LaunchRequestHandler.HandleRequest()] Generating initial audio only response");
                    return ResponseBuilder.Ask(output, null);
                }

            }, logger);
        }
    }
}
