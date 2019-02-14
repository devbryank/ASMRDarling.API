using System.Threading.Tasks;

using Amazon.Lambda.Core;

using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;
using ASMRDarling.API.Data;
using ASMRDarling.API.Models;
using ASMRDarling.API.Helpers;
using ASMRDarling.API.Interfaces;

namespace ASMRDarling.API.Handlers
{
    /// <summary>
    /// This class processes the built in intent request
    /// </summary>
    class BuiltInIntentHandler : IBuiltInIntentHandler
    {
        public Task<SkillResponse> HandleIntent(Intent intent, Session session, ILambdaLogger logger)
        {
            return RequestProcessor.ProcessAlexaRequest("BuiltInIntentHandler.HandleIntent()", "Built In Intent", async () =>
            {
                // Declare response components to return
                SkillResponse response = new SkillResponse();
                SsmlOutputSpeech output = new SsmlOutputSpeech();
                ProgressiveResponse progressiveResponse = session.Attributes["quick_response"] as ProgressiveResponse;


                // Get display availability
                bool? hasDisplay = session.Attributes["has_display"] as bool?;


                // Get most recently played media item
                string currentMedia = null;

                //if (hasDisplay == true)
                //{
                //    currentMedia = session.Attributes["current_video_item"] as string;
                //}
                //else
                //{
                currentMedia = session.Attributes["current_audio_item"] as string;
                //}

                MediaItem currentMediaItem = MediaItems.GetMediaItems().Find(m => m.FileName.Contains(currentMedia));


                // Get sub intent name
                string subIntentType = session.Attributes["sub_intent"] as string;


                // Direct intent into the appropriate handler
                switch (subIntentType)
                {
                    // Handle help intent
                    case AlexaConstants.BuiltInHelp:
                        logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating a {AlexaConstants.BuiltInHelp} response, type of {intent.Name}");
                        output = SsmlTemplate.HelpSpeech();
                        response = ResponseBuilder.Ask(output, null);
                        break;

                        //    // handle next intent
                        //    case NextSuffix:
                        //        logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating a {NextSuffix} response, type of {intent.Name}");

                        //        // get next media item
                        //        MediaItem nextMediaItem = MediaItems.GetMediaItems().Find(m => m.Id.Equals(currentMediaItem.Id + 1));
                        //        logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Current media item title: {currentMediaItem.Title}");

                        //        // send a quick response before loading the content
                        //        await progressiveResponse.SendSpeech($"Playing next, {nextMediaItem.Title}");

                        //        // set next response
                        //        if (nextMediaItem != null)
                        //        {
                        //            // if next media item is available
                        //            logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Next media item title: {nextMediaItem.Title}");

                        //            if (hasDisplay == true)
                        //            {
                        //                response = ResponseBuilder.Empty();
                        //                response.Response.Directives.Add(new VideoAppDirective(nextMediaItem.VideoSource));
                        //            }
                        //            else
                        //            {
                        //                response = ResponseBuilder.Empty();
                        //                response = ResponseBuilder.AudioPlayerPlay(PlayBehavior.ReplaceAll, nextMediaItem.AudioSource, nextMediaItem.FileName);
                        //            }
                        //        }
                        //        else
                        //        {
                        //            // if next media item is not available
                        //            logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Next media item is not available");

                        //            output = SsmlTemplate.MediaPlayerNoNextSpeech();
                        //            response = ResponseBuilder.Tell(output);
                        //        }
                        //        break;

                        //    // handle previous intent
                        //    case PreviousSuffix:
                        //        logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating a {PreviousSuffix} response, type of {intent.Name}");

                        //        // get previous media item
                        //        MediaItem previousMediaItem = MediaItems.GetMediaItems().Find(m => m.Id.Equals(currentMediaItem.Id - 1));
                        //        logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Current media item title: {currentMediaItem.Title}");

                        //        // send a quick response before loading the content
                        //        await progressiveResponse.SendSpeech($"Playing previous, {previousMediaItem.Title}");

                        //        // set previous response
                        //        if (previousMediaItem != null)
                        //        {
                        //            // if previous media item is available
                        //            logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Next media item title: {previousMediaItem.Title}");

                        //            if (hasDisplay == true)
                        //            {
                        //                response = ResponseBuilder.Empty();
                        //                response.Response.Directives.Add(new VideoAppDirective(previousMediaItem.VideoSource));
                        //            }
                        //            else
                        //            {
                        //                response = ResponseBuilder.Empty();
                        //                response = ResponseBuilder.AudioPlayerPlay(PlayBehavior.ReplaceAll, previousMediaItem.AudioSource, previousMediaItem.FileName);
                        //            }
                        //        }
                        //        else
                        //        {
                        //            // if previous media item is not available
                        //            logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Previous media item is not available");

                        //            output = SsmlTemplate.MediaPlayerNoPreviousSpeech();
                        //            response = ResponseBuilder.Tell(output);
                        //        }
                        //        break;

                        //    // handle resume intent
                        //    case ResumeSuffix:
                        //        logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating a {ResumeSuffix} response, type of {intent.Name}");

                        //        // set resume response
                        //        //response = ResponseBuilder.AudioPlayerPlay();
                        //        break;

                        //    // handle pause intent
                        //    case PauseSuffix:
                        //        logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating a {PauseSuffix} response, type of {intent.Name}");

                        //        // set pause command and response
                        //        if (hasDisplay == true)
                        //        {
                        //            var aplPauseCommand = new ExecuteCommandsDirective("Pause APL Video Player", new[]
                        //            {
                        //                new ControlMedia
                        //                {
                        //                    Command = ControlMediaCommand.Pause,
                        //                    ComponentId = "apl_video_player"
                        //                }
                        //            });

                        //            response.Response.Directives.Add(aplPauseCommand);
                        //        }
                        //        else
                        //            response = ResponseBuilder.AudioPlayerStop();

                        //        break;

                        //    // handle stop intent
                        //    case StopSuffix:
                        //        logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating a {StopSuffix} response, type of {intent.Name}");

                        //        // set stop response
                        //        response = ResponseBuilder.AudioPlayerStop();
                        //        break;

                        //    // handle default case
                        //    default:
                        //        logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating a defailt response, type of {intent.Name}");
                        //        output = SsmlTemplate.MediaPlayerControlSpeech();
                        //        response = ResponseBuilder.Ask(output, null);
                        //        break;
                        //}
                }

                // Return response
                return response;

            }, logger);
        }
    }
}
