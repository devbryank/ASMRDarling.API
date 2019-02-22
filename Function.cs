using Newtonsoft.Json;
using System.Threading.Tasks;

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
    // entry point for lamba function
    public class Function
    {
        public async Task<SkillResponse> FunctionHandler(APLSkillRequest input, ILambdaContext context)
        {
            // core initialization
            await Core.Init(input, context);


            // direct request in a matching handler
            switch (Core.Request.GetMainRequestType())
            {
                // handle launch reqeust
                case AlexaRequestType.LaunchRequest:
                    LaunchRequestHandler launchRequestHandler = new LaunchRequestHandler();
                    await launchRequestHandler.HandleRequest();
                    break;

                // handle intent request
                case AlexaRequestType.IntentRequest:
                    IntentReqeustHandler intentRequestHandler = new IntentReqeustHandler();
                    await intentRequestHandler.HandleRequest();
                    break;

                // handle audio player request
                case AlexaRequestType.AudioPlayerRequest:
                    AudioPlayerRequestHandler audioPlayerRequestHandler = new AudioPlayerRequestHandler();
                    await audioPlayerRequestHandler.HandleRequest();
                    break;

                // handle gui event request
                case AlexaRequestType.AlexaRequest:
                    AlexaRequestHandler alexaRequestHandler = new AlexaRequestHandler();
                    await alexaRequestHandler.HandleRequest();
                    break;

                // handle session eneded request
                case AlexaRequestType.SessionEndedRequest:
                    SessionEndedRequestHandler sessionEndedRequestHandler = new SessionEndedRequestHandler();
                    await sessionEndedRequestHandler.HandleRequest();
                    break;

                // handle system exception request
                case AlexaRequestType.SystemRequest:
                    SystemRequestHandler systemRequestHandler = new SystemRequestHandler();
                    await systemRequestHandler.HandleRequest();
                    break;

                // handle unknown request
                default:
                    bool endSession = Core.State.UserState.NumReprompt > 4 ? true : false;
                    if (endSession)
                        Core.State.UserState.NumReprompt = 0;
                    Core.Logger.Write("Function.FunctionHandler()", "Request was not recognized, directing into the default case handler");
                    Core.Response.SetSpeech(false, endSession, SpeechTemplate.NoUnderstand);
                    Core.State.UserState.Stage = Stage.Menu;
                    Core.State.UserState.NumReprompt++;
                    if (endSession)
                        Core.State.UserState.NumReprompt = 0;
                    break;
            }


            // log returning message details
            Core.Logger.Write("Function.FunctionHandler()", $"Response details: {JsonConvert.SerializeObject(Core.Response.GetResponse())}");
            Core.Logger.Write("Function.FunctionHandler()", $"User state details: {JsonConvert.SerializeObject(Core.State)}");


            // save database context then return
            await Core.Database.SaveState();
            return Core.Response.GetResponse();
        }
    }
}
