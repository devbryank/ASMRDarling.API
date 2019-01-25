using System.Threading.Tasks;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;

namespace ASMRDarling.API.Interfaces
{
    public interface IRequestHandler
    {
        Task<SkillResponse> HandleRequest(LaunchRequest request);
        Task<SkillResponse> HandleRequest(IntentRequest request);
        Task<SkillResponse> HandleRequest(AudioPlayerRequest request);
    }
}
