using System.Threading.Tasks;

using Sonnar.Helpers;
using Sonnar.Components;
using Sonnar.Interfaces;
using Sonnar.Templates;

namespace Sonnar.Handlers
{
    class IntentReqeustHandler : IRequestHandler
    {
        public async Task HandleRequest()
        {
            await RequestProcessHelper.ProcessRequest("IntentRequestHandler.HandleRequest()", "Intent Request", async () =>
            {
                string mainIntentType = Core.Request.GetMainIntentType();

                switch (mainIntentType)
                {
                    // handle built in intent
                    case AlexaRequests.BuiltInIntent:
                        BuiltInIntentHandler builtInIntentHandler = new BuiltInIntentHandler();
                        await builtInIntentHandler.HandleRequest();
                        break;

                    // handle custom intent
                    case AlexaRequests.CustomIntent:
                        CustomIntentHandler customIntentHandler = new CustomIntentHandler();
                        await customIntentHandler.HandleRequest();
                        break;

                    // handle unknown intent
                    default:
                        Core.Logger.Write("IntentRequestHandler.HandleRequest()", $"Intent was not recognized, directing into the default intent fallback");
                        Core.Response.SetTellSpeech(SpeechTemplate.IntentUnknown);
                        break;
                }
            });
        }
    }
}
