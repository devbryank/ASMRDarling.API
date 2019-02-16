using Alexa.NET.Request;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using ASMRDarling.API.Interfaces;
using ASMRDarling.API.Models;
using System;
using System.Threading.Tasks;

namespace ASMRDarling.API.Handlers
{
    class AudioPlayerRequestHandler : IAudioPlayerRequestHandler
    {
        public Task<SkillResponse> HandleRequest(SkillRequest input, MediaState currentState, Session session, ILambdaLogger logger)
        {
            throw new NotImplementedException();
        }
    }
}
