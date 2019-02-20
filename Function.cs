using System.Threading.Tasks;
using Newtonsoft.Json;

using Amazon.Lambda.Core;

using Alexa.NET.Request;
using Alexa.NET.Response;

using Sonnar;
using Sonnar.Handlers;
using Sonnar.Templates;
using Sonnar.Components;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AsmrDarlingAlexaSkill
{
    public class Function
    {
        public async Task<SkillResponse> FunctionHandler(APLSkillRequest input, ILambdaContext context)
        {
            // core library initialization
            await Core.Init(input, context);


            // direct request into the matching handler
            switch (Core.Request.GetMainRequestType())
            {
                // handle launch request
                case AlexaRequests.LaunchRequest:
                    LaunchRequestHandler launchRequestHandler = new LaunchRequestHandler();
                    await launchRequestHandler.HandleRequest();
                    break;

                // handle intent request
                case AlexaRequests.IntentRequest:
                    IntentReqeustHandler intentRequestHandler = new IntentReqeustHandler();
                    await intentRequestHandler.HandleRequest();
                    break;

                // handle audio player request
                case AlexaRequests.AudioPlayerRequest:
                    AudioPlayerRequestHandler audioPlayerRequestHandler = new AudioPlayerRequestHandler();
                    await audioPlayerRequestHandler.HandleRequest();
                    break;

                // handle gui user event request
                case AlexaRequests.AlexaRequest:
                    AlexaRequestHandler alexaRequestHandler = new AlexaRequestHandler();
                    await alexaRequestHandler.HandleRequest();
                    break;

                // handle session ended request
                case AlexaRequests.SessionEndedRequest:
                    SessionEndedRequestHandler sessionEndedRequestHandler = new SessionEndedRequestHandler();
                    await sessionEndedRequestHandler.HandleRequest();
                    break;

                // handle system exception request
                case AlexaRequests.SystemRequest:
                    SystemRequestHandler systemRequestHandler = new SystemRequestHandler();
                    await systemRequestHandler.HandleRequest();
                    break;

                // handle unknown request
                default:
                    Core.Logger.Write("Function.FunctionHandler()", "Request was not recognized, directing into the default request fallback");
                    Core.Response.SetTellSpeech(SpeechTemplate.RequestUnknown);
                    break;
            }


            //Core.Logger.Write("Function.FunctionHandler()", $"User state details: {JsonConvert.SerializeObject(Core.State)}");
            //Core.Logger.Write("Function.FunctionHandler()", $"Response details: {JsonConvert.SerializeObject(Core.Response.GetResponse())}");


            // save database context
            await Core.Database.SaveState();

            // return response to alexa
            return Core.Response.GetResponse();
        }
    }
}
