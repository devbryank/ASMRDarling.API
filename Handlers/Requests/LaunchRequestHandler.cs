using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using ASMRDarling.API.Interfaces;

namespace ASMRDarling.API.Handlers
{
    class LaunchRequestHandler : ILaunchRequestHandler
    {
        public LaunchRequestHandler() { }


        public async Task<SkillResponse> HandleRequest(LaunchRequest request, ILambdaLogger logger)
        {
            logger.LogLine($"[LaunchRequestHandler.HandleRequest()] Launch request handling started");

            var output = new SsmlOutputSpeech()
            {
                Ssml = "<speak>" +
                            "<amazon:effect name='whispered'>" +
                                 "<prosody rate='slow'>" +
                                      "<p>Hey,</p>" +
                                      "<p>it's me.</p>" +
                                      "<p>ASMR Darling.</p>" +
                                      "<p>To begin,</p>" +
                                      "<p>you can say things like,</p>" +
                                      "<p>play 10 triggers to help you sleep,</p>" +
                                      "<p>or just say play 10 triggers.</p>" +
                                 "</prosody>" +
                            "</amazon:effect>" +
                       "</speak>"
            };

            return ResponseBuilder.Ask(output, null);
        }


        public async Task<SkillResponse> HandleRequest(IntentRequest request, Session session, ILambdaLogger logger)
        {
            throw new NotImplementedException();
        }


        public async Task<SkillResponse> HandleRequest(AudioPlayerRequest request, ILambdaLogger logger)
        {
            throw new NotImplementedException();
        }
    }
}
