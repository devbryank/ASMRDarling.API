using System.Threading.Tasks;

using Amazon.Lambda.Core;

using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using ASMRDarling.API.Data;
using ASMRDarling.API.Helpers;
using ASMRDarling.API.Interfaces;

namespace ASMRDarling.API.Handlers
{
    /// <summary>
    /// This class processes the intent request
    /// </summary>
    class IntentRequestHandler : IIntentRequestHandler
    {
        // Intent handlers
        readonly IBuiltInIntentHandler builtInIntentHandler = new BuiltInIntentHandler();
        readonly IPlayAsmrIntentHandler playAsmrIntentHandler = new PlayAsmrIntentHandler();


        public Task<SkillResponse> HandleRequest(SkillRequest input, Session session, ILambdaLogger logger)
        {
            return RequestProcessor.ProcessAlexaRequest("IntentRequestHandler.HandleRequest()", "Intent Request", async () =>
            {
                // Get intent request
                var request = input.Request as IntentRequest;


                // Declare response components to return
                Intent intent = request.Intent;
                SkillResponse response = new SkillResponse();
                SsmlOutputSpeech output = new SsmlOutputSpeech();


                // Get intent type
                string intentType = intent.Name;
                string[] intentTypes = intent.Name.Split(".");
                string mainIntentType = intentTypes[0];
                string subIntentType = intentTypes[intentTypes.Length - 1];

                logger.LogLine($"[IntentRequestHandler.HandleRequest()] Main intent type: {mainIntentType}");

                if (subIntentType != null && !subIntentType.Equals(mainIntentType))
                    logger.LogLine($"[IntentRequestHandler.HandleRequest()] Sub intent type: {subIntentType} derived from {intentType}");

                session.Attributes["sub_intent"] = subIntentType;


                // Direct intent into the appropriate handler
                switch (mainIntentType)
                {
                    // Handle built in intent
                    case AlexaConstants.BuiltIn:
                        logger.LogLine($"[IntentRequestHandler.HandleRequest()] Directing intent into {AlexaConstants.BuiltIn} handler");
                        response = await builtInIntentHandler.HandleIntent(intent, session, logger);
                        break;


                    // Handle play media intent
                    case AlexaConstants.PlayASMR:
                        logger.LogLine($"[IntentRequestHandler.HandleRequest()] Directing intent into {AlexaConstants.PlayASMR} handler");
                        response = await playAsmrIntentHandler.HandleIntent(intent, session, logger);
                        break;


                    // Handle default fallback case
                    default:
                        logger.LogLine($"[IntentRequestHandler.HandleRequest()] Intent was not recognized, directing intent into the default case handler");
                        output = SsmlTemplate.FallbackSpeech();
                        response = ResponseBuilder.Tell(output);
                        break;
                }

                // Return response
                return response;

            }, logger);
        }
    }
}
