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
        //const string FallbackSuffix = "FallbackIntent";
        const string HelpSuffix = "HelpIntent";
        //const string CalcelSuffix = "CancelIntent";
        const string NextSuffix = "NextIntent";  // context audio player token has the current clip name save into the session on function
        const string PreviousSuffix = "PreviousIntent";
        //const string RepeatSuffix = "Repeat";
        const string ResumeSuffix = "ResumeIntent";
        const string PauseSuffix = "PauseIntent";
        //const string StartOverSuffix = "StartOver";
        const string StopSuffix = "StopIntent";


        public BuiltInIntentHandler() { }


        public async Task<SkillResponse> HandleIntent(Intent intent, Session session, ILambdaLogger logger)
        {
            var intentNamePartials = intent.Name.Split('.');
            string suffix = intentNamePartials[intentNamePartials.Length - 1];

            SkillResponse response = new SkillResponse();
            SsmlOutputSpeech output = new SsmlOutputSpeech();

            switch (suffix)
            {
                case HelpSuffix:
                    logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating a response for {intent.Name}, type of {suffix}");

                    output = SsmlBuilder.HelpSpeech();
                    response = ResponseBuilder.Ask(output, null);
                    break;

                //case CalcelSuffix:
                //    logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating a response for {intent.Name}, type of {suffix}");

                //    response = ResponseBuilder.AudioPlayerStop();
                //    break;

                case PauseSuffix:
                    logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating a response for {intent.Name}, type of {suffix}");

                    response = ResponseBuilder.AudioPlayerStop();
                    break;

                default:
                    logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating a response for {intent.Name}, type of unassigned (default)");

                    output = SsmlBuilder.BuildSpeech("<speak><amazon:effect name='whispered'><prosody rate='slow'>While a media is in play, you can say help, next, previous, resume, pause, stop to control it.</prosody></amazon:effect></speak>");
                    response = ResponseBuilder.Tell(output, null);
                    break;
            }

            return response;
        }
    }
}
