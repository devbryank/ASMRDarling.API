using Alexa.NET.Request;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using ASMRDarling.API.Interfaces;
using ASMRDarling.API.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
