using System;
using System.Threading.Tasks;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using ASMRDarling.API.Interfaces;

namespace ASMRDarling.API.Handlers
{
    public class LaunchRequestHandler : ILaunchRequestHandler
    {
        public async Task<SkillResponse> HandleRequest(LaunchRequest request)
        {
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


        public async Task<SkillResponse> HandleRequest(IntentRequest request)
        {
            throw new NotImplementedException();
        }


        public async Task<SkillResponse> HandleRequest(AudioPlayerRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
