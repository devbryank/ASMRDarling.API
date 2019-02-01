using Amazon.Lambda.Core;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;
using ASMRDarling.API.Interfaces;

namespace ASMRDarling.API.Handlers
{
    class AudioPlayerIntentHandler : IAudioPlayerIntentHandler
    {
        public Task<SkillResponse> HandleIntent(Intent intent, Session session, ILambdaLogger logger)
        {
            return null;
        }
    }
}
