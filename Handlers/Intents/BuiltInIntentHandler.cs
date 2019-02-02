using Amazon.Lambda.Core;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Alexa.NET.Response.Directive;
using ASMRDarling.API.Models;
using ASMRDarling.API.Templates;
using ASMRDarling.API.Interfaces;

namespace ASMRDarling.API.Handlers
{
    class BuiltInIntentHandler : IBuiltInIntentHandler
    {

#warning context audio player token has the current clip name save into the session on function 
#warning make a constant.cs to store alexa related constants

        // Intent suffix constants
        const string HelpSuffix = "HelpIntent";
        const string NextSuffix = "NextIntent";
        const string PreviousSuffix = "PreviousIntent";
        const string ResumeSuffix = "ResumeIntent";
        const string PauseSuffix = "PauseIntent";
        const string StopSuffix = "StopIntent";


        // Constructor
        public BuiltInIntentHandler() { }


        // Intent handler start
        public async Task<SkillResponse> HandleIntent(Intent intent, Session session, ILambdaLogger logger)
        {
            logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Built in intent handling started");

            // Check the display availability
            bool? hasDisplay = session.Attributes["has_display"] as bool?;

            // Get the most recently played media item
            string currentClip = session.Attributes["current_clip"] as string;
            MediaItem currentMediaItem = MediaItems.GetMediaItems().Find(m => m.FileName.Contains(currentClip));

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

                    // Set help response
                    output = SsmlTemplate.HelpSpeech();
                    response = ResponseBuilder.Ask(output, null);
                    break;

                // Handle next intent
                case NextSuffix:
                    logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating a {NextSuffix} response, type of {intent.Name}");

                    // Get next media item
                    MediaItem nextMediaItem = MediaItems.GetMediaItems().Find(m => m.Id.Equals(currentMediaItem.Id + 1));
                    logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Current clip title: {currentMediaItem.Title}");

                    // Set next response
                    if (nextMediaItem != null)
                    {
                        // If next media item is available
                        logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Next clip title: {nextMediaItem.Title}");

                        if (hasDisplay == true)
                        {
                            response = ResponseBuilder.Empty();
                            response.Response.Directives.Add(new VideoAppDirective(nextMediaItem.VideoSource));
                        }
                        else
                        {
                            response = ResponseBuilder.Empty();
                            response = ResponseBuilder.AudioPlayerPlay(PlayBehavior.ReplaceAll, nextMediaItem.AudioSource, nextMediaItem.FileName);
                        }
                    }
                    else
                    {
                        // If next media item is not available
                        logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Next clip is not available");

                        output = SsmlTemplate.MediaPlayerNoNextSpeech();
                        response = ResponseBuilder.Tell(output);
                    }
                    break;

                // Handle previous intent
                case PreviousSuffix:
                    logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating a {PreviousSuffix} response, type of {intent.Name}");

                    // Get previous media item
                    MediaItem previousMediaItem = MediaItems.GetMediaItems().Find(m => m.Id.Equals(currentMediaItem.Id - 1));
                    logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Current clip title: {currentMediaItem.Title}");

                    // Set previous response
                    if (previousMediaItem != null)
                    {
                        // If previous media item is available
                        logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Next clip title: {previousMediaItem.Title}");

                        if (hasDisplay == true)
                        {
                            response = ResponseBuilder.Empty();
                            response.Response.Directives.Add(new VideoAppDirective(previousMediaItem.VideoSource));
                        }
                        else
                        {
                            response = ResponseBuilder.Empty();
                            response = ResponseBuilder.AudioPlayerPlay(PlayBehavior.ReplaceAll, previousMediaItem.AudioSource, previousMediaItem.FileName);
                        }
                    }
                    else
                    {
                        // If previous media item is not available
                        logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Previous clip is not available");

                        output = SsmlTemplate.MediaPlayerNoPreviousSpeech();
                        response = ResponseBuilder.Tell(output);
                    }
                    break;

                // Handle resume intent
                case ResumeSuffix:
                    logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating a {ResumeSuffix} response, type of {intent.Name}");

                    // Set resume response
                    //response = ResponseBuilder.AudioPlayerPlay();
                    break;

                // Handle pause intent
                case PauseSuffix:
                    logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating a {PauseSuffix} response, type of {intent.Name}");

                    // Set pause response
                    response = ResponseBuilder.AudioPlayerStop();
                    break;

                // Handle stop intent
                case StopSuffix:
                    logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating a {StopSuffix} response, type of {intent.Name}");

                    // Set stop response
                    response = ResponseBuilder.AudioPlayerStop();
                    break;

                // Handle default case
                default:
                    logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating a defailt response, type of {intent.Name}");
                    output = SsmlTemplate.MediaPlayerControlSpeech();
                    response = ResponseBuilder.Ask(output, null);
                    break;
            }

            // Return response to the intent request handler
            return response;
        }
    }
}
