using System.Threading.Tasks;

using Alexa.NET.APL;

using Sonnar.Helpers;
using Sonnar.Templates;
using Sonnar.Components;
using Sonnar.Interfaces;

namespace Sonnar.Handlers
{
    class AlexaRequestHandler : IRequestHandler
    {
        public async Task HandleRequest()
        {
            await RequestProcessHelper.ProcessRequest("AlexaRequestHandler.HandleRequest()", "Alexa Request", async () =>
            {
                string subRequestType = Core.Request.GetSubRequestType();

                switch (subRequestType)
                {
                    // handle user gui event request
                    case AlexaRequests.UserEventRequest:
                        UserEventRequest userEventRequest = Core.Request.GetRequest().Request as UserEventRequest;

                        string title = userEventRequest.Arguments[0] as string;
                        string url = userEventRequest.Arguments[1] as string;

                        Core.Logger.Write("AlexaRequestHandler.HandleRequest()", $"Media file title: {title}");
                        Core.Logger.Write("AlexaRequestHandler.HandleRequest()", $"Media file source url: {url}");

                        Core.Response.AddVideoApp(url, title, title);
                        break;

                    // handle unknown intent
                    default:
                        Core.Response.SetTellSpeech(SpeechTemplate.IntentUnknown);
                        break;
                }
            });
        }
    }
}
