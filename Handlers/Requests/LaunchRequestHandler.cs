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


                // Save user state
                session.Attributes["user_state"] = UserStates.Menu;
                string userState = session.Attributes["user_state"] as string;
                logger.LogLine($"[LaunchRequestHandler.HandleRequest()] User state updated to: {userState}");


                // Get session display value, then set an output speech
                bool ? hasDisplay = session.Attributes["has_display"] as bool?;
                SsmlOutputSpeech output = SsmlTemplate.LaunchSpeech(hasDisplay);

                if (hasDisplay == true)
                {
                    // If the device has a display interface
                    logger.LogLine($"[LaunchRequestHandler.HandleRequest()] Generating initial APL response");

                    // Get APL response then return
                    SkillResponse response = ResponseBuilder.Ask(output, null, session);
                    return await AplTemplate.MenuDisplay(response);
                }
                else
                {
                    // Audio only response
                    logger.LogLine($"[LaunchRequestHandler.HandleRequest()] Generating initial audio only response");
                    return ResponseBuilder.Ask(output, null, session);
                }

            }, logger);
        }
    }
}
