using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using ASMRDarling.API.Interfaces;

namespace ASMRDarling.API.Handlers
{
    class IntentRequestHandler : IIntentRequestHandler
    {
        const string MEDIA_INTENT_NAME = "PlayMedia";

        IPlayMediaIntentHandler _playMediaIntentHandler;


        public IntentRequestHandler() { _playMediaIntentHandler = new PlayMediaIntentHandler(); }


        public async Task<SkillResponse> HandleRequest(IntentRequest request, Session session, ILambdaLogger logger)
        {
            logger.LogLine($"[IntentRequestHandler.HandleRequest()] Intent request handling started");
            logger.LogLine($"[IntentRequestHandler.HandleRequest()] Intent requested: {request.Intent.Name}");

            Intent intent = request.Intent;
            SkillResponse response = new SkillResponse();

            switch (intent.Name)
            {
                case MEDIA_INTENT_NAME:
                    response = await _playMediaIntentHandler.HandleIntent(intent, session, logger);
                    break;
                default:
                    var output = new SsmlOutputSpeech()
                    {
                        Ssml = "<speak>" +
                                    "<amazon:effect name='whispered'>" +
                                         "<prosody rate='slow'>" +
                                              "Sorry, I didn't get your intention, can you please tell me one more time?." +
                                         "</prosody>" +
                                    "</amazon:effect>" +
                               "</speak>"
                    };

                    response = ResponseBuilder.Ask(output, null);
                    break;
            }

            return response;
        }


        public Task<SkillResponse> HandleRequest(LaunchRequest request, ILambdaLogger logger)
        {
            throw new System.NotImplementedException();
        }


        public Task<SkillResponse> HandleRequest(AudioPlayerRequest request, ILambdaLogger logger)
        {
            throw new System.NotImplementedException();
        }
    }
}
