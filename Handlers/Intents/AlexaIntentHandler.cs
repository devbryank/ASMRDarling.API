using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;
using ASMRDarling.API.Builders;
using ASMRDarling.API.Interfaces;

namespace ASMRDarling.API.Handlers
{
    public class AlexaIntentHandler : IAlexaIntentHandler
    {
        const string UserEventSuffix = "UserEvent";


        public AlexaIntentHandler() { }


        public async Task<SkillResponse> HandleIntent(Intent intent, Session session, ILambdaLogger logger)
        {
            var intentNamePartials = intent.Name.Split('.');
            string suffix = intentNamePartials[intentNamePartials.Length - 1];

            SkillResponse response = new SkillResponse();
            SsmlOutputSpeech output = new SsmlOutputSpeech();

            switch (suffix)
            {
                case UserEventSuffix:
                    logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating a response for {intent.Name}, type of {suffix}");

                    //output = SsmlBuilder.HelpSpeech();
                    var argument = session.Attributes["arguments"] as string;
                    //response = await LaunchRequestHandler.GetAplVideo(argument);
                    break;
            }

            return response;
        }
    }
}
