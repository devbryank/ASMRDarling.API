using System.Threading.Tasks;

using Amazon.Lambda.Core;

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
        //readonly IPlayMediaIntentHandler playMediaIntentHandler;
        readonly IBuiltInIntentHandler builtInIntentHandler = new BuiltInIntentHandler();


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

                if (subIntentType != null || !subIntentType.Equals(mainIntentType))
                    logger.LogLine($"[IntentRequestHandler.HandleRequest()] Sub intent type: {subIntentType} derived from {intentType}");

                session.Attributes["sub_intent"] = subIntentType;


                // Direct intent into the appropriate handler
                switch (mainIntentType)
                {
                    // Handle play media intent
                    //case AlexaConstants.PlayASMR:
                    //    logger.LogLine($"[IntentRequestHandler.HandleRequest()] Directing intent into {AlexaConstants.PlayASMR} handler");
                    //    response = await playMediaIntentHandler.HandleIntent(intent, session, logger);
                    //    break;

                    case AlexaConstants.BuiltIn:
                        logger.LogLine($"[IntentRequestHandler.HandleRequest()] Directing intent into {AlexaConstants.BuiltIn} handler");
                        response = await builtInIntentHandler.HandleIntent(intent, session, logger);
                        break;








                        //// handle other intents (built in, exception)
                        //default:
                        //    var intentNamePartials = intent.Name.Split('.');

                        //    if (intentNamePartials[0].Equals(BuiltInIntentName))
                        //    {
                        //        // handle built in intent
                        //        logger.LogLine($"[IntentRequestHandler.HandleRequest()] Directing intent into {BuiltInIntentName} handler");
                        //        response = await builtInIntentHandler.HandleIntent(intent, session, logger);
                        //    }
                        //    else
                        //    {
                        //        // handle default case, without any recognizable intent
                        //        logger.LogLine($"[IntentRequestHandler.HandleRequest()] Intent was not recognized, directing intent into the default case");
                        //        output = SsmlTemplate.ExceptionSpeech();
                        //        response = ResponseBuilder.Tell(output);
                        //    }
                        //    break;
                }

                // Return response
                return response;

            }, logger);
        }
    }
}
