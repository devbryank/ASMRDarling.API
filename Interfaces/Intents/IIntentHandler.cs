using System.Threading.Tasks;

using Amazon.Lambda.Core;

using Alexa.NET.Request;
using Alexa.NET.Response;

namespace ASMRDarling.API.Interfaces
{
    interface IIntentHandler
    {
        Task<SkillResponse> HandleIntent(Intent intent, Session session, ILambdaLogger logger);
    }
}
