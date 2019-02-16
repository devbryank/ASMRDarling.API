using Amazon.Lambda.Core;
using System;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;
using ASMRDarling.API.Models;
using ASMRDarling.API.Interfaces;

namespace ASMRDarling.API.Handlers
{
    class SessionEndedRequestHandler : ISessionEndedRequestHandler
    {
        public Task<SkillResponse> HandleRequest(SkillRequest input, MediaState currentState, Session session, ILambdaLogger logger)
        {
            throw new NotImplementedException();
        }
    }
}
