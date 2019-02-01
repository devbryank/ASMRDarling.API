using Amazon.Lambda.Core;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;
using ASMRDarling.API.Interfaces;

namespace ASMRDarling.API.Handlers
{
    class SessionEndedRequestHandler : ISessionEndedRequestHandler
    {
        // Constructor
        public SessionEndedRequestHandler() { }


        // Request handler
        public Task<SkillResponse> HandleRequest(SkillRequest input, Session session, ILambdaLogger logger)
        {
            logger.LogLine($"[SessionEndedRequestHandler.HandleRequest()] Session Ended request handling started");

            return null;
        }
    }
}
