using Amazon.Lambda.Core;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;
using ASMRDarling.API.Models;

namespace ASMRDarling.API.Interfaces
{
    internal interface IRequestHandler
    {
        Task<SkillResponse> HandleRequest(SkillRequest input, MediaState currentState, Session session, ILambdaLogger logger);
    }
}
