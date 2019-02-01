using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;
using ASMRDarling.API.Builders;
using ASMRDarling.API.Templates;
using ASMRDarling.API.Interfaces;

namespace ASMRDarling.API.Handlers
{
    class BuiltInIntentHandler : IBuiltInIntentHandler
    {
#warning context audio player token has the current clip name save into the session on function 
        const string HelpSuffix = "HelpIntent";
        const string NextSuffix = "NextIntent";
        const string PreviousSuffix = "PreviousIntent";
        const string ResumeSuffix = "ResumeIntent";
        const string PauseSuffix = "PauseIntent";
        const string StopSuffix = "StopIntent";


        // Constructor
        public BuiltInIntentHandler() { }


        // Intent handler
        public async Task<SkillResponse> HandleIntent(Intent intent, Session session, ILambdaLogger logger)
        {
            logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Built in intent handling started");

            // Split intent name to get the suffix only
            var intentNamePartials = intent.Name.Split('.');
            string suffix = intentNamePartials[intentNamePartials.Length - 1];

            // Declare response to return
            SkillResponse response = new SkillResponse();
            SsmlOutputSpeech output = new SsmlOutputSpeech();

            // Direct intent into the matching handler
            switch (suffix)
            {
                // Handle help intent
                case HelpSuffix:
                    logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating a {HelpSuffix} response, type of {intent.Name}");

                    // Return help response
                    output = SsmlTemplate.HelpSpeech();
                    response = ResponseBuilder.Ask(output, null);
                    break;

                // Handle next intent
                case NextSuffix:
                    logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating a {NextSuffix} response, type of {intent.Name}");

                    // Return next response
                    output = SsmlTemplate.HelpSpeech();
                    response = ResponseBuilder.Ask(output, null);
                    break;

                // Handle previous intent
                case PreviousSuffix:
                    logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating a {PreviousSuffix} response, type of {intent.Name}");

                    // Return previous response
                    //response = ResponseBuilder.AudioPlayerStop();
                    break;

                // Handle resume intent
                case ResumeSuffix:
                    logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating a {ResumeSuffix} response, type of {intent.Name}");

                    // Return resume response
                    //response = ResponseBuilder.AudioPlayerPlay();
                    break;

                // Handle pause intent
                case PauseSuffix:
                    logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating a {PauseSuffix} response, type of {intent.Name}");

                    // Return pause response
                    response = ResponseBuilder.AudioPlayerStop();
                    break;

                // Handle stop intent
                case StopSuffix:
                    logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating a {StopSuffix} response, type of {intent.Name}");

                    // Return stop response
                    //response = ResponseBuilder.AudioPlayerStop();
                    //kill the skill
                    break;

                default:
                    logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating a defailt response, type of {intent.Name}");
                    output = SsmlTemplate.AudioPlayerSpeech();
                    response = ResponseBuilder.Tell(output, null);
                    break;
            }

            return response;
        }
    }
}
