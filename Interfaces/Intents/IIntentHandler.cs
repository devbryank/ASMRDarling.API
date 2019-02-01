using Amazon.Lambda.Core;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace ASMRDarling.API.Interfaces
{
    interface IIntentHandler
    {
        Task<SkillResponse> HandleIntent(Intent intent, Session session, ILambdaLogger logger);
    }
}
