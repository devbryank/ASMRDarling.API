using Amazon.Lambda.Core;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using ASMRDarling.API.Data;
using ASMRDarling.API.Models;
using ASMRDarling.API.Helpers;
using ASMRDarling.API.Interfaces;

namespace ASMRDarling.API.Handlers
{
    /// <summary>
    /// this class processes intent request
    /// </summary>
    class IntentRequestHandler : IIntentRequestHandler
    {
        public Task<SkillResponse> HandleRequest(SkillRequest input, MediaState currentState, Session session, ILambdaLogger logger)
        {
            return RequestProcessHelper.ProcessAlexaRequest("[IntentRequestHandler.HandleRequest()]", "Intent Request", async () =>
            {
                SkillResponse response = new SkillResponse();
                SsmlOutputSpeech output = new SsmlOutputSpeech();
                IntentRequest request = input.Request as IntentRequest;
                Intent intent = request.Intent;


                // get main & sub intent types
                string intentType = intent.Name;
                string[] intentTypes = intent.Name.Split(".");
                string mainIntentType = intentTypes[0];
                string subIntentType = intentTypes.Length > 1 ? intentTypes[intentTypes.Length - 1] : null;

                logger.LogLine($"[IntentRequestHandler.HandleRequest()] Main intent type: <{mainIntentType}>");
                if (subIntentType != null)
                    logger.LogLine($"[IntentRequestHandler.HandleRequest()] Sub intent type: <{subIntentType}> derived from {intentType}");
                session.Attributes["sub_intent"] = subIntentType;


                // direct intent into an appropriate handler
                switch (mainIntentType)
                {
                    // handle built-in intent
                    case AlexaRequestConstants.BuiltIn:
                        IBuiltInIntentHandler builtInIntentHandler = new BuiltInIntentHandler();
                        logger.LogLine($"[IntentRequestHandler.HandleRequest()] Directing intent into {AlexaRequestConstants.BuiltIn} handler");
                        response = await builtInIntentHandler.HandleIntent(intent, currentState, session, logger);
                        break;


                    // handle play asmr intent
                    case AlexaRequestConstants.PlayAsmr:
                        IPlayAsmrIntentHandler playAsmrIntentHandler = new PlayAsmrIntentHandler();
                        logger.LogLine($"[IntentRequestHandler.HandleRequest()] Directing intent into {AlexaRequestConstants.PlayAsmr} handler");
                        response = await playAsmrIntentHandler.HandleIntent(intent, currentState, session, logger);
                        break;


                    // handle default fallback case
                    default:
                        logger.LogLine($"[IntentRequestHandler.HandleRequest()] Intent was not recognized, directing intent into the default case handler");
                        output = SsmlTemplate.RequestFallbackSpeech();
                        response = ResponseBuilder.Ask(output, null);
                        break;
                }


                // return response back to function handler
                return response;

            }, logger);
        }
    }
}
