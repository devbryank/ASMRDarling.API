using System.Threading.Tasks;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;

namespace ASMRDarling.API.Interfaces
{
    public interface IIntentHandler
    {
        Task<SkillResponse> HandleIntent(IntentRequest input);
    }
}
