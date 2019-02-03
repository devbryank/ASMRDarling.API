using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;
using ASMRDarling.API.Templates;
using ASMRDarling.API.Interfaces;

namespace ASMRDarling.API.Handlers
{
    class SessionEndedRequestHandler : ISessionEndedRequestHandler
    {
        public SessionEndedRequestHandler() { }

        public async Task<SkillResponse> HandleRequest(SkillRequest input, Session session, ILambdaLogger logger)
        {
            try
            {
                logger.LogLine($"[SessionEndedRequestHandler.HandleRequest()] Session Ended request handling started");

                // declare response to return
                var response = ResponseBuilder.Empty();
                response.Response.ShouldEndSession = true;

                logger.LogLine($"[SessionEndedRequestHandler.HandleRequest()] Session ended");

                // return response to the function handler
                return response;
            }
            catch (Exception ex)
            {
                logger.LogLine($"[SessionEndedRequestHandler.HandleRequest()] Exception caught");
                logger.LogLine($"[SessionEndedRequestHandler.HandleRequest()] Exception log: {ex}");

                // return system exception to the function handler
                var output = SsmlTemplate.SystemFaultSpeech();
                return ResponseBuilder.Tell(output);
            }
        }
    }
}
