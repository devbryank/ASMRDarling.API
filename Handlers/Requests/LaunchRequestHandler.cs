using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using ASMRDarling.API.Templates;
using ASMRDarling.API.Interfaces;

namespace ASMRDarling.API.Handlers
{
    class LaunchRequestHandler : ILaunchRequestHandler
    {
        public LaunchRequestHandler() { }

        public async Task<SkillResponse> HandleRequest(SkillRequest input, Session session, ILambdaLogger logger)
        {
            try
            {
                logger.LogLine($"[LaunchRequestHandler.HandleRequest()] Launch Request handling started");

                // get launch request
                LaunchRequest launchRequest = input.Request as LaunchRequest;

                // get session display value, then make output speech
                bool? hasDisplay = session.Attributes["has_display"] as bool?;
                SsmlOutputSpeech output = SsmlTemplate.LaunchSpeech(hasDisplay);

                if (hasDisplay == true)
                {
                    // if the device has display
                    logger.LogLine($"[LaunchRequestHandler.HandleRequest()] Generating initial APL response");

                    // get apl response then return
                    var response = ResponseBuilder.Ask(output, null);
                    return await AplTemplate.MenuDisplay(response);
                }
                else
                {
                    // if display is not available
                    logger.LogLine($"[LaunchRequestHandler.HandleRequest()] Generating initial audio only response");

                    // return audio only response
                    return ResponseBuilder.Ask(output, null);
                }
            }
            catch (Exception ex)
            {
                logger.LogLine($"[LaunchRequestHandler.HandleRequest()] Exception caught");
                logger.LogLine($"[LaunchRequestHandler.HandleRequest()] Exception log: {ex}");

                // return system exception to the function handler
                var output = SsmlTemplate.SystemFaultSpeech();
                return ResponseBuilder.Tell(output);
            }
        }
    }
}
