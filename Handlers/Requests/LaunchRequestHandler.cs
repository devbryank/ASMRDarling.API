using Amazon.Lambda.Core;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using ASMRDarling.API.Data;
using ASMRDarling.API.Models;
using ASMRDarling.API.Helpers;
using ASMRDarling.API.Interfaces;

namespace ASMRDarling.API.Handlers
{
    /// <summary>
    /// this class processes launch request
    /// </summary>
    class LaunchRequestHandler : ILaunchRequestHandler
    {
        public Task<SkillResponse> HandleRequest(SkillRequest input, MediaState currentState, Session session, ILambdaLogger logger)
        {
            return RequestProcessHelper.ProcessAlexaRequest("[LaunchRequestHandler.HandleRequest()]", "Launch Request", async () =>
            {
                SkillResponse response = new SkillResponse();
                LaunchRequest launchRequest = input.Request as LaunchRequest;

                currentState.State.State = UserStateConstants.Menu;
                logger.LogLine($"[LaunchRequestHandler.HandleRequest()] User state updated to: {currentState.State.State}");

                bool? hasDisplay = session.Attributes["has_display"] as bool?;
                SsmlOutputSpeech output = SsmlTemplate.LaunchSpeech(hasDisplay);
                response = ResponseBuilder.Ask(output, null);

                logger.LogLine($"[LaunchRequestHandler.HandleRequest()] Generating initial response");
                response = hasDisplay == true ? await AplTemplate.MenuDisplay(response, logger) : response;

                return response;

            }, logger);
        }
    }
}
