using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace ASMRDarling.API.Interfaces
{
    interface IRequestHandler
    {
        Task<SkillResponse> HandleRequest(SkillRequest input, Session session, ILambdaLogger logger);
    }
}
