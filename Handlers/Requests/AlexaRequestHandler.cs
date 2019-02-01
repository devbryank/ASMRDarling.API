using Amazon.Lambda.Core;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;
using ASMRDarling.API.Interfaces;

namespace ASMRDarling.API.Handlers
{
    class AlexaRequestHandler : IAlexaRequestHandler
    {
        // Constructor
        public AlexaRequestHandler() { }


        // Request handler
        public Task<SkillResponse> HandleRequest(SkillRequest input, Session session, ILambdaLogger logger)
        {
            logger.LogLine($"[AlexaRequestHandler.HandleRequest()] Alexa request handling started");

            return null;
        }
    }
}
