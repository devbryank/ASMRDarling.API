using Amazon.Lambda.Core;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace ASMRDarling.API.Interfaces
{
    interface IRequestHandler
    {
        Task<SkillResponse> HandleRequest(SkillRequest request, Session session, ILambdaLogger logger);
    }
}
