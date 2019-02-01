using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using ASMRDarling.API.Builders;
using ASMRDarling.API.Interfaces;

namespace ASMRDarling.API.Handlers
{
    class IntentRequestHandler : IIntentRequestHandler
    {
        const string ListIntentName = "ListMedia";
        const string PlayIntentName = "PlayMedia";
        const string BuiltInIntentName = "AMAZON";

        const string AlexaIntentName = "Alexa";

        // update new list media intent to give the list of clips to users


        IPlayMediaIntentHandler _playMediaIntentHandler;
        IBuiltInIntentHandler _builtInIntentHandler;
        IAlexaIntentHandler _alexaIntentHandler;

        //AudioPlayer.ClearQueue
        //AudioPlayer.PlaybackStarted   say playing filename
        //AudioPlayer.PlaybackStopped  say play paused
        //AudioPlayer.PlaybackFinished
        //AudioPlayer.PlaybackFailed   should add a fallback?


        public IntentRequestHandler()
        {
            _playMediaIntentHandler = new PlayMediaIntentHandler();
            _builtInIntentHandler = new BuiltInIntentHandler();
            _alexaIntentHandler = new AlexaIntentHandler();
        }


        public async Task<SkillResponse> HandleRequest(IntentRequest request, Session session, ILambdaLogger logger)
        {
            logger.LogLine($"[IntentRequestHandler.HandleRequest()] Intent request handling started");
            logger.LogLine($"[IntentRequestHandler.HandleRequest()] Intent requested: {request.Intent.Name}");

            Intent intent = request.Intent;
            SkillResponse response = new SkillResponse();
            SsmlOutputSpeech output = new SsmlOutputSpeech();

            switch (intent.Name)
            {
                case ListIntentName:
                    logger.LogLine($"[IntentRequestHandler.HandleRequest()] {ListIntentName} processing started");

                    break;

                case PlayIntentName:
                    logger.LogLine($"[IntentRequestHandler.HandleRequest()] {PlayIntentName} processing started");

                    response = await _playMediaIntentHandler.HandleIntent(intent, session, logger);
                    break;

                default:
                    var intentNamePartials = intent.Name.Split('.');

                    if (intentNamePartials[0].Equals(BuiltInIntentName))
                    {
                        logger.LogLine($"[IntentRequestHandler.HandleRequest()] {BuiltInIntentName} processing started");

                        response = await _builtInIntentHandler.HandleIntent(intent, session, logger);
                    }
                    else if (intentNamePartials[0].Equals(AlexaIntentName))
                    {
                        logger.LogLine($"[IntentRequestHandler.HandleRequest()] {AlexaIntentName} processing started");

                        response = await _alexaIntentHandler.HandleIntent(intent, session, logger);

                    }
                    else
                    {
                        logger.LogLine($"[IntentRequestHandler.HandleRequest()] Intent was not recognized");
                        output = SsmlBuilder.ExceptionSpeech();
                        response = ResponseBuilder.Ask(output, null);
                    }

                    break;
            }

            return response;
        }


        public Task<SkillResponse> HandleRequest(LaunchRequest request, Session session, ILambdaLogger logger)
        {
            throw new System.NotImplementedException();
        }


        public Task<SkillResponse> HandleRequest(AudioPlayerRequest request, Session session, ILambdaLogger logger)
        {
            throw new System.NotImplementedException();
        }

        public Task<SkillResponse> HandleRequest(SkillRequest request, Session session, ILambdaLogger logger)
        {
            throw new System.NotImplementedException();
        }
    }
}
