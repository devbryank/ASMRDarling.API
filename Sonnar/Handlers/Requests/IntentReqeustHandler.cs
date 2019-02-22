using System.Threading.Tasks;

using Sonnar.Helpers;
using Sonnar.Templates;
using Sonnar.Components;
using Sonnar.Interfaces;

namespace Sonnar.Handlers
{
    class IntentReqeustHandler : IRequestHandler
    {
        public async Task HandleRequest()
        {
            await RequestProcessHelper.ProcessRequest("IntentRequestHandler.HandleRequest()", "Intent Request", async () =>
            {
                // get main intent type
                string mainIntentType = Core.Request.GetMainIntentType();


                // direct intent into a matching handler
                switch (mainIntentType)
                {
                    // handle built in intent
                    case AlexaRequestType.BuiltInIntent:
                        BuiltInIntentHandler builtInIntentHandler = new BuiltInIntentHandler();
                        await builtInIntentHandler.HandleRequest();
                        break;

                    // handle custom intent
                    case AlexaRequestType.CustomIntent:
                        CustomIntentHandler customIntentHandler = new CustomIntentHandler();
                        await customIntentHandler.HandleRequest();
                        break;

                    // handle unknown intent
                    default:
                        bool endSession = Core.State.UserState.NumReprompt > 4 ? true : false;
                        Core.Logger.Write("IntentRequestHandler.HandleRequest()", "Intent was not recognized, directing into the default case handler");
                        Core.Response.SetSpeech(false, endSession, SpeechTemplate.NoUnderstand);
                        Core.State.UserState.Stage = Stage.Menu;
                        Core.State.UserState.NumReprompt++;
                        if (endSession)
                            Core.State.UserState.NumReprompt = 0;
                        break;
                }
            });
        }
    }
}
