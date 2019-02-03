using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using ASMRDarling.API.Templates;
using ASMRDarling.API.Interfaces;

namespace ASMRDarling.API.Handlers
{
    class IntentRequestHandler : IIntentRequestHandler
    {
        // intent names
        const string PlayIntentName = "PlayMedia";
        const string BuiltInIntentName = "AMAZON";

        // intent handlers
        readonly IPlayMediaIntentHandler playMediaIntentHandler;
        readonly IBuiltInIntentHandler builtInIntentHandler;


        public IntentRequestHandler()
        {
            playMediaIntentHandler = new PlayMediaIntentHandler();
            builtInIntentHandler = new BuiltInIntentHandler();
        }


        public async Task<SkillResponse> HandleRequest(SkillRequest input, Session session, ILambdaLogger logger)
        {
            try
            {
                logger.LogLine($"[IntentRequestHandler.HandleRequest()] Intent Request handling started");

                // get intent request
                var request = input.Request as IntentRequest;
                logger.LogLine($"[IntentRequestHandler.HandleRequest()] Intent requested: {request.Intent.Name}");

                // declare response to return
                Intent intent = request.Intent;
                SkillResponse response = new SkillResponse();
                SsmlOutputSpeech output = new SsmlOutputSpeech();

                // direct intent into the matching handler
                switch (intent.Name)
                {
                    // handle play media intent
                    case PlayIntentName:
                        logger.LogLine($"[IntentRequestHandler.HandleRequest()] Directing intent into {PlayIntentName} handler");
                        response = await playMediaIntentHandler.HandleIntent(intent, session, logger);
                        break;

                    // handle other intents (built in, exception)
                    default:
                        var intentNamePartials = intent.Name.Split('.');

                        if (intentNamePartials[0].Equals(BuiltInIntentName))
                        {
                            // handle built in intent
                            logger.LogLine($"[IntentRequestHandler.HandleRequest()] Directing intent into {BuiltInIntentName} handler");
                            response = await builtInIntentHandler.HandleIntent(intent, session, logger);
                        }
                        else
                        {
                            // handle default case, without any recognizable intent
                            logger.LogLine($"[IntentRequestHandler.HandleRequest()] Intent was not recognized, directing intent into the default case");
                            output = SsmlTemplate.ExceptionSpeech();
                            response = ResponseBuilder.Tell(output);
                        }
                        break;
                }

                // return response to the function handler
                return response;
            }
            catch (Exception ex)
            {
                logger.LogLine($"[IntentRequestHandler.HandleRequest()] Exception caught");
                logger.LogLine($"[IntentRequestHandler.HandleRequest()] Exception log: {ex}");

                // return system exception to the function handler
                var output = SsmlTemplate.SystemFaultSpeech();
                return ResponseBuilder.Tell(output);
            }
        }
    }
}
