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
    class AudioPlayerRequestHandler : IAudioPlayerRequestHandler
    {
        // request suffix constants
        const string ClearQueueSuffix = "ClearQueue";
        const string PlaybackStartedSuffix = "PlaybackStarted";
        const string PlaybackStoppedSuffix = "PlaybackStopped";
        const string PlaybackNearlyFinished = "PlaybackNearlyFinished";
        const string PlaybackFinished = "PlaybackFinished";
        const string PlaybackFaild = "PlaybackFailed";


        public AudioPlayerRequestHandler() { }

        public async Task<SkillResponse> HandleRequest(SkillRequest input, Session session, ILambdaLogger logger)
        {
            try
            {
                logger.LogLine($"[AudioPlayerRequestHandler.HandleRequest()] Audio Player request handling started");

                // get the most recently played media item and the next one
                string currentMedia = session.Attributes["current_audio_item"] as string;
                MediaItem currentMediaItem = MediaItems.GetMediaItems().Find(m => m.FileName.Contains(currentMedia));
                MediaItem nextMediaItem = MediaItems.GetMediaItems().Find(m => m.Id.Equals(currentMediaItem.Id + 1));

                // split request name to get the suffix only
                var requestNamePartials = input.Request.Type.Split('.');
                string suffix = requestNamePartials[requestNamePartials.Length - 1];

                // declare response to return
                SkillResponse response = new SkillResponse();
                SsmlOutputSpeech output = new SsmlOutputSpeech();

                // direct request into the matching handler
                switch (suffix)
                {
                    // handle playback started intent
                    case PlaybackStartedSuffix:
                        logger.LogLine($"[AudioPlayerRequestHandler.HandleRequest()] Directing request into {PlaybackStartedSuffix} handler");

                        response = ResponseBuilder.Empty();
                        break;

                    // handle playback stopped intent
                    case PlaybackStoppedSuffix:
                        logger.LogLine($"[AudioPlayerRequestHandler.HandleRequest()] Directing request into {PlaybackStoppedSuffix} handler");
                        logger.LogLine($"[AudioPlayerRequestHandler.HandleRequest()] Audio player stopped");

                        response = ResponseBuilder.Empty();
                        break;

                    // handle playback nearly finished intent
                    case PlaybackNearlyFinished:
                        logger.LogLine($"[AudioPlayerRequestHandler.HandleRequest()] Directing request into {PlaybackNearlyFinished} handler");

                        // enqueue the next item at the current media's near end
                        if (nextMediaItem != null)
                            response = ResponseBuilder.AudioPlayerPlay(PlayBehavior.Enqueue, nextMediaItem.AudioSource, nextMediaItem.Title);
                        else
                        {
                            output = SsmlTemplate.MediaPlayerNoNextSpeech();
                            response = ResponseBuilder.Tell(output, null);
                        }
                        break;

                    // handle playback finished intent
                    case PlaybackFinished:
                        logger.LogLine($"[AudioPlayerRequestHandler.HandleRequest()] Directing request into {PlaybackFinished} handler");

                        // play the next item after the current media's end
                        if (nextMediaItem != null)
                            response = ResponseBuilder.AudioPlayerPlay(PlayBehavior.ReplaceAll, nextMediaItem.AudioSource, nextMediaItem.Title);
                        else
                        {
                            output = SsmlTemplate.MediaPlayerNoNextSpeech();
                            response = ResponseBuilder.Tell(output, null);
                        }
                        break;

                    // handle playback failed intent
                    case PlaybackFaild:
                        logger.LogLine($"[AudioPlayerRequestHandler.HandleRequest()] Directing request into {PlaybackFaild} handler");
                        logger.LogLine($"[AudioPlayerRequestHandler.HandleRequest()] Audio player failed to play the media");

                        output = SsmlTemplate.ExceptionSpeech();
                        response = ResponseBuilder.Tell(output, null);
                        break;
                }

                // return response to the function handler
                return response;
            }
            catch (Exception ex)
            {
                logger.LogLine($"[AudioPlayerRequestHandler.HandleRequest()] Exception caught");
                logger.LogLine($"[AudioPlayerRequestHandler.HandleRequest()] Exception log: {ex}");

                // return system exception to the function handler
                var output = SsmlTemplate.SystemFaultSpeech();
                return ResponseBuilder.Tell(output);
            }
        }
    }
}
