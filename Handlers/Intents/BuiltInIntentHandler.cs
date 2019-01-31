using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;
using ASMRDarling.API.Builders;
using ASMRDarling.API.Interfaces;

namespace ASMRDarling.API.Handlers
{
    class BuiltInIntentHandler : IBuiltInIntentHandler
    {
        const string HelpSuffix = "Help";
        const string CalcelSuffix = "Cancel";
        const string NextSuffix = "Next";
        const string PreviousSuffix = "Previous";
        const string RepeatSuffix = "Repeat";
        const string ResumeSuffix = "Resume";
        const string PauseSuffix = "Pause";
        const string StartOverSuffix = "StartOver";
        const string StopSuffix = "Stop";

        public BuiltInIntentHandler() { }


        public async Task<SkillResponse> HandleIntent(Intent intent, Session session, ILambdaLogger logger)
        {
            var intentNamePartials = intent.Name.Split('.');
            string suffix = intentNamePartials[1];

            SkillResponse response = new SkillResponse();
            SsmlOutputSpeech output = new SsmlOutputSpeech();

            switch (suffix) {
                case HelpSuffix:
                    logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating a response for {intent.Name}, type of {suffix}");

                    output = SsmlBuilder.HelpSpeech();
                    response = ResponseBuilder.Ask(output, null);
                    break;

                case CalcelSuffix:
                    logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating a response for {intent.Name}, type of {suffix}");

                    response = ResponseBuilder.AudioPlayerStop();
                    break;

                case NextSuffix:
                    logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating a response for {intent.Name}, type of {suffix}");

                    response = ResponseBuilder.AudioPlayerStop();
                    break;
            }

            return response;
        }
    }
}
