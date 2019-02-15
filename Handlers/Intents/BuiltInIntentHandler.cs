using System.Threading.Tasks;

using Amazon.Lambda.Core;

using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Alexa.NET.Response.Directive;
using ASMRDarling.API.Templates;
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
        public Task<SkillResponse> HandleIntent(Intent intent, MediaState currentState, Session session, ILambdaLogger logger)
        {
            return RequestProcessor.ProcessAlexaRequest("BuiltInIntentHandler.HandleIntent()", "Built In Intent", async () =>
            {
                // Declare response components to return
                SkillResponse response = new SkillResponse();
                SsmlOutputSpeech output = new SsmlOutputSpeech();
                ProgressiveResponse progressiveResponse = session.Attributes["quick_response"] as ProgressiveResponse;


                // Get session attributes
                bool? hasDisplay = session.Attributes["has_display"] as bool?;
                string userState = session.Attributes["user_state"] as string;
                string subIntentType = session.Attributes["sub_intent"] as string;
                logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] User state: {userState}");

#warning implement video
                // Get most recently played media item
                string currentMedia = null;

                //if (hasDisplay == true)
                //{
                //    currentMedia = session.Attributes["current_video_item"] as string;
                //}
                //else
                //{
                currentMedia = currentState.State.Token;
                //}

                MediaItem currentMediaItem = MediaItems.GetMediaItems().Find(m => m.FileName.Contains(currentMedia));


                // Direct intent into the appropriate handler
                if (currentState.State.State != "MENU_MODE" || subIntentType.Equals(AlexaConstants.BuiltInHelp))
                {
                    switch (subIntentType)
                    {
                        // Handle help intent
                        case AlexaConstants.BuiltInHelp:
                            logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating a {AlexaConstants.BuiltInHelp} response, type of {intent.Name}");
                            output = SsmlTemplate.HelpSpeech();
                            response = ResponseBuilder.Ask(output, null, session);
                            break;


                        // Handle next intent
                        case AlexaConstants.BuiltInNext:
                            logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating a {AlexaConstants.BuiltInNext} response, type of {intent.Name}");

                            // Get next media item
                            MediaItem nextMediaItem = MediaItems.GetMediaItems().Find(m => m.Id.Equals(currentMediaItem.Id + 1));
                            logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Current media item title: {currentMediaItem.Title}");


                            // Set next response
                            if (nextMediaItem != null)
                            {
                                // Send a quick response before loading the content
                                await progressiveResponse.SendSpeech($"Playing next media, {nextMediaItem.Title}");

                                logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Next media item title: {nextMediaItem.Title}");

                                if (hasDisplay == true)
                                {
                                    response = ResponseBuilder.Empty();
                                    response.Response.Directives.Add(new VideoAppDirective(nextMediaItem.VideoSource));
                                }
                                else
                                {

                                    currentState.State.Index = nextMediaItem.Id;
                                    currentState.State.Token = nextMediaItem.FileName;
                                    response = ResponseBuilder.Empty();
                                    response = ResponseBuilder.AudioPlayerPlay(PlayBehavior.ReplaceAll, nextMediaItem.AudioSource, nextMediaItem.FileName);
                                }
                            }
                            else
                            {
                                logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Next media item is not available");

                                output = SsmlTemplate.NoNextMediaSpeech();
                                response = ResponseBuilder.Ask(output, null, session);
                            }
                            break;


                        // Handle previous intent
                        case AlexaConstants.BuiltInPrevious:
                            logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating a {AlexaConstants.BuiltInPrevious} response, type of {intent.Name}");

                            // Get previous media item
                            MediaItem previousMediaItem = MediaItems.GetMediaItems().Find(m => m.Id.Equals(currentMediaItem.Id - 1));
                            logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Current media item title: {currentMediaItem.Title}");


                            // Set previous response
                            if (previousMediaItem != null)
                            {
                                // Send a quick response before loading the content
                                await progressiveResponse.SendSpeech($"Playing previous, {previousMediaItem.Title}");

                                logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Next media item title: {previousMediaItem.Title}");

                                if (hasDisplay == true)
                                {
                                    response = ResponseBuilder.Empty();
                                    response.Response.Directives.Add(new VideoAppDirective(previousMediaItem.VideoSource));
                                }
                                else
                                {
                                    currentState.State.Token = previousMediaItem.FileName;
                                    currentState.State.Index = previousMediaItem.Id;
                                    response = ResponseBuilder.Empty();
                                    response = ResponseBuilder.AudioPlayerPlay(PlayBehavior.ReplaceAll, previousMediaItem.AudioSource, previousMediaItem.FileName);
                                }
                            }
                            else
                            {
                                logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Previous media item is not available");

                                output = SsmlTemplate.NoPreviousMediaSpeech();
                                response = ResponseBuilder.Ask(output, null, session);
                            }
                            break;


                        // Handle resume intent
                        //case AlexaConstants.BuiltInResume:
                        //    logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating a {AlexaConstants.BuiltInResume} response, type of {intent.Name}");
                        //    response = ResponseBuilder.AudioPlayerPlay(PlayBehavior.ReplaceAll, null, );
                        //    break;

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


                        // Handle default fallback case
                        default:
                            logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating a default fallback response");
                            output = SsmlTemplate.ControlMediaSpeech();
                            response = ResponseBuilder.Ask(output, null, session);
                            break;
                    }
                }
                else
                {
                    logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Unable to process {subIntentType}, user is at {userState}");
                    logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating a default fallback response");
                    output = SsmlTemplate.CommandFallbackSpeech();
                    response = ResponseBuilder.Ask(output, null, session);
                }

                // Return response
                return response;

            }, logger);
        }
    }
}
