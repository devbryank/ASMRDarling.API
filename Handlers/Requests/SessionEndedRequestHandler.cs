using Amazon.Lambda.Core;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;
using ASMRDarling.API.Interfaces;

namespace ASMRDarling.API.Handlers
{
    class SessionEndedRequestHandler : ISessionEndedRequestHandler
    {
        // Constructor
        public SessionEndedRequestHandler() { }


        // Request handler start
        public async Task<SkillResponse> HandleRequest(SkillRequest input, Session session, ILambdaLogger logger)
        {
            logger.LogLine($"[SessionEndedRequestHandler.HandleRequest()] Session Ended request handling started");

            // Declare response to return
            var response = ResponseBuilder.Empty();
            response.Response.ShouldEndSession = true;

            logger.LogLine($"[SessionEndedRequestHandler.HandleRequest()] Session Ended.");

            // Return response to the function handler
            return response;
        }
    }
}
