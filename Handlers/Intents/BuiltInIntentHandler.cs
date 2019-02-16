using Amazon.Lambda.Core;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Alexa.NET.Response.Directive;
using ASMRDarling.API.Data;
using ASMRDarling.API.Models;
using ASMRDarling.API.Helpers;
using ASMRDarling.API.Interfaces;

namespace ASMRDarling.API.Handlers
{
    /// <summary>
    /// this class processes built-in intent
    /// </summary>
    class BuiltInIntentHandler : IBuiltInIntentHandler
    {
        public Task<SkillResponse> HandleIntent(Intent intent, MediaState currentState, Session session, ILambdaLogger logger)
        {
            return RequestProcessHelper.ProcessAlexaRequest("[BuiltInIntentHandler.HandleIntent()]", "Built In Intent", async () =>
            {
                SkillResponse response = new SkillResponse();
                SsmlOutputSpeech output = new SsmlOutputSpeech();
                ProgressiveResponse progressiveResponse = session.Attributes["quick_response"] as ProgressiveResponse;

                bool? hasDisplay = session.Attributes["has_display"] as bool?;
                string subIntentType = session.Attributes["sub_intent"] as string;


                // get most recently played media item
                int currentMediaIndex = currentState.State.Index;
                MediaItem currentMediaItem = MediaItems.GetMediaItems().Find(m => m.Id.Equals(currentMediaIndex));


                // direct intent into an appropriate handler
                if (currentState.State.State != UserStateConstants.Menu || subIntentType.Equals(AlexaRequestConstants.BuiltInHelp))
                {
                    switch (subIntentType)
                    {
                        // handle help intent
                        case AlexaRequestConstants.BuiltInHelp:
                            logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating <{AlexaRequestConstants.BuiltInHelp}> response, type of {intent.Name}");
                            output = SsmlTemplate.HelpSpeech();
                            response = ResponseBuilder.Ask(output, null);
                            break;


                        // handle next intent
                        case AlexaRequestConstants.BuiltInNext:
                            logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating <{AlexaRequestConstants.BuiltInNext}> response, type of {intent.Name}");

                            // get next media item
                            MediaItem nextMediaItem = MediaItems.GetMediaItems().Find(m => m.Id.Equals(currentMediaItem.Id + 1));
                            logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Current media item title: {currentMediaItem.Title}");

                            // set next response
                            if (nextMediaItem != null)
                            {
                                // send a quick response before loading the content
                                await progressiveResponse.SendSpeech($"Playing next media item, {nextMediaItem.Title}");

                                logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Next media item title in queue: {nextMediaItem.Title}");

                                if (hasDisplay == true)
                                {

#warning need to be fixed, it won't be played
                                    response = ResponseBuilder.Empty();
                                    response.Response.Directives.Add(new VideoAppDirective(nextMediaItem.VideoSource));
                                }
                                else
                                {
                                    currentState.State.Index = nextMediaItem.Id;
                                    currentState.State.Token = nextMediaItem.FileName;
                                    currentState.State.OffsetInMS = 0;

                                    response = ResponseBuilder.Empty();
                                    response = ResponseBuilder.AudioPlayerPlay(PlayBehavior.ReplaceAll, nextMediaItem.AudioSource, nextMediaItem.FileName);
                                }
                            }
                            else
                            {
                                logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Next media item is not available");
                                output = SsmlTemplate.NoNextMediaSpeech();
                                response = ResponseBuilder.Ask(output, null);
                            }
                            break;


                        // handle previous intent
                        case AlexaRequestConstants.BuiltInPrevious:
                            logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating <{AlexaRequestConstants.BuiltInPrevious}> response, type of {intent.Name}");

                            // get previous media item
                            MediaItem previousMediaItem = MediaItems.GetMediaItems().Find(m => m.Id.Equals(currentMediaItem.Id - 1));
                            logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Current media item title: {currentMediaItem.Title}");

                            // set previous response
                            if (previousMediaItem != null)
                            {
                                // send a quick response before loading the content
                                await progressiveResponse.SendSpeech($"Playing previous media item, {previousMediaItem.Title}");

                                logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Next media item title in queue: {previousMediaItem.Title}");

                                if (hasDisplay == true)
                                {

#warning need to be fixed, it won't be played
                                    response = ResponseBuilder.Empty();
                                    response.Response.Directives.Add(new VideoAppDirective(previousMediaItem.VideoSource));
                                }
                                else
                                {
                                    currentState.State.Index = previousMediaItem.Id;
                                    currentState.State.Token = previousMediaItem.FileName;
                                    currentState.State.OffsetInMS = 0;

                                    response = ResponseBuilder.Empty();
                                    response = ResponseBuilder.AudioPlayerPlay(PlayBehavior.ReplaceAll, previousMediaItem.AudioSource, previousMediaItem.FileName);
                                }
                            }
                            else
                            {
                                logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Previous media item is not available");
                                output = SsmlTemplate.NoPreviousMediaSpeech();
                                response = ResponseBuilder.Ask(output, null);
                            }
                            break;


                        //// handle resume intent
                        //case AlexaRequestConstants.BuiltInResume:
                        //    logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating <{AlexaRequestConstants.BuiltInResume}> response, type of {intent.Name}");

                        //    if (hasDisplay == true)

                        //    else
                        //        response = ResponseBuilder.AudioPlayerPlay(PlayBehavior.ReplaceAll, currentMediaItem.AudioSource, currentMediaItem.FileName, currentState.State.OffsetInMS);
                        //    break;


                        //// handle pause intent
                        //case AlexaRequestConstants.BuiltInPause:
                        //    logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating <{AlexaRequestConstants.BuiltInPause}> response, type of {intent.Name}");

                        //    // set pause response
                        //    if (hasDisplay == true)

                        //    else
                        //        response = ResponseBuilder.AudioPlayerStop();

                        //    break;


                        //// handle stop intent
                        //case AlexaRequestConstants.BuiltInStop:
                        //    logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating <{AlexaRequestConstants.BuiltInStop}> response, type of {intent.Name}");

                        //    // set stop response
                        //    if (hasDisplay == true)

                        //    else
                        //        response = ResponseBuilder.AudioPlayerStop();
                        //    break;


                        // handle default fallback case
                        default:
                            logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Intent was not recognized, generating a default fallback case response");
                            output = SsmlTemplate.ControlMediaSpeech();
                            response = ResponseBuilder.Ask(output, null);
                            break;
                    }
                }
                else
                {
                    logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Unable to process <{subIntentType}>, user is at {currentState.State.State}");
                    logger.LogLine($"[BuiltInIntentHandler.HandleIntent()] Generating a default command fallback case response");
                    output = SsmlTemplate.CommandFallbackSpeech();
                    response = ResponseBuilder.Ask(output, null, session);
                }


                // return response back to intent request handler
                return response;

            }, logger);
        }
    }
}
