using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
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

#warning make a constant.cs to store alexa related constants

        // intent suffix constants
        const string HelpSuffix = "HelpIntent";
        const string NextSuffix = "NextIntent";
        const string PreviousSuffix = "PreviousIntent";
        const string ResumeSuffix = "ResumeIntent";
        const string PauseSuffix = "PauseIntent";
        const string StopSuffix = "StopIntent";


        public BuiltInIntentHandler() { }

        public async Task<SkillResponse> HandleIntent(Intent intent, Session session, ILambdaLogger logger)
        {
            try
            {
                logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Built In Intent handling started");

                // check the display availability
                bool? hasDisplay = session.Attributes["has_display"] as bool?;

                // get the most recently played media item
                string currentClip = session.Attributes["current_clip"] as string;
                MediaItem currentMediaItem = MediaItems.GetMediaItems().Find(m => m.FileName.Contains(currentClip));

                // split intent name to get the suffix only
                var intentNamePartials = intent.Name.Split('.');
                string suffix = intentNamePartials[intentNamePartials.Length - 1];

                // declare response to return
                SkillResponse response = new SkillResponse();
                SsmlOutputSpeech output = new SsmlOutputSpeech();
                ProgressiveResponse progressiveResponse = session.Attributes["quick_response"] as ProgressiveResponse;

                // direct intent into the matching handler
                switch (suffix)
                {
                    // handle help intent
                    case HelpSuffix:
                        logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating a {HelpSuffix} response, type of {intent.Name}");

                        // set help response
                        output = SsmlTemplate.HelpSpeech();
                        response = ResponseBuilder.Ask(output, null);
                        break;

                    // handle next intent
                    case NextSuffix:
                        logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating a {NextSuffix} response, type of {intent.Name}");

                        // get next media item
                        MediaItem nextMediaItem = MediaItems.GetMediaItems().Find(m => m.Id.Equals(currentMediaItem.Id + 1));
                        logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Current media item title: {currentMediaItem.Title}");

                        // send a quick response before loading the content
                        await progressiveResponse.SendSpeech($"Playing next, {nextMediaItem.Title}");

                        // set next response
                        if (nextMediaItem != null)
                        {
                            // if next media item is available
                            logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Next media item title: {nextMediaItem.Title}");

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
                            // if next media item is not available
                            logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Next media item is not available");

                            output = SsmlTemplate.MediaPlayerNoNextSpeech();
                            response = ResponseBuilder.Tell(output);
                        }
                        break;

                    // handle previous intent
                    case PreviousSuffix:
                        logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating a {PreviousSuffix} response, type of {intent.Name}");

                        // get previous media item
                        MediaItem previousMediaItem = MediaItems.GetMediaItems().Find(m => m.Id.Equals(currentMediaItem.Id - 1));
                        logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Current media item title: {currentMediaItem.Title}");

                        // send a quick response before loading the content
                        await progressiveResponse.SendSpeech($"Playing previous, {previousMediaItem.Title}");

                        // set previous response
                        if (previousMediaItem != null)
                        {
                            // if previous media item is available
                            logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Next media item title: {previousMediaItem.Title}");

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
                            // if previous media item is not available
                            logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Previous media item is not available");

                            output = SsmlTemplate.MediaPlayerNoPreviousSpeech();
                            response = ResponseBuilder.Tell(output);
                        }
                        break;

                    // handle resume intent
                    case ResumeSuffix:
                        logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating a {ResumeSuffix} response, type of {intent.Name}");

                        // set resume response
                        //response = ResponseBuilder.AudioPlayerPlay();
                        break;

                    // handle pause intent
                    case PauseSuffix:
                        logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating a {PauseSuffix} response, type of {intent.Name}");

                        // set pause response
                        response = ResponseBuilder.AudioPlayerStop();
                        break;

                    // handle stop intent
                    case StopSuffix:
                        logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating a {StopSuffix} response, type of {intent.Name}");

                        // set stop response
                        response = ResponseBuilder.AudioPlayerStop();
                        break;

                    // handle default case
                    default:
                        logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating a defailt response, type of {intent.Name}");
                        output = SsmlTemplate.MediaPlayerControlSpeech();
                        response = ResponseBuilder.Ask(output, null);
                        break;
                }

                // return response to the intent request handler
                return response;
            }
            catch (Exception ex)
            {
                logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Exception caught");
                logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Exception log: {ex}");

                // return system exception to the intent request handler
                var output = SsmlTemplate.SystemFaultSpeech();
                return ResponseBuilder.Tell(output);
            }
        }
    }
}
