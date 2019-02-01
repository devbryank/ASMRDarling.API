// Refactoring Done

using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Amazon.Lambda.Core;

namespace ASMRDarling.API.Interfaces
{
    public interface IRequestHandler
    {
        Task<SkillResponse> HandleRequest(SkillRequest request, Session session, ILambdaLogger logger);
    }
}
