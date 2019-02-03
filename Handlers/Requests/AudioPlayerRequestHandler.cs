using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Alexa.NET.Request;
using Alexa.NET.Response;
using ASMRDarling.API.Interfaces;
using ASMRDarling.API.Models;
using Alexa.NET;
using ASMRDarling.API.Templates;

namespace ASMRDarling.API.Handlers
{
    class AudioPlayerRequestHandler : IAudioPlayerRequestHandler
    {
        // request suffix constants
        const string ClearQueueSuffix = "ClearQueue";
        const string PlaybackStartedSuffix = "PlaybackStarted";
        const string PlaybackStoppedSuffix = "PlaybackStopped";
        const string PlaybackFinished = "PlaybackFinished";
        const string PlaybackFaild = "PlaybackFailed";


        public AudioPlayerRequestHandler() { }

        public async Task<SkillResponse> HandleRequest(SkillRequest input, Session session, ILambdaLogger logger)
        {
            try
            {
                logger.LogLine($"[AudioPlayerRequestHandler.HandleRequest()] Audio Player request handling started");

                // get the most recently played media item
                string currentClip = session.Attributes["current_clip"] as string;
                MediaItem currentMediaItem = MediaItems.GetMediaItems().Find(m => m.FileName.Contains(currentClip));

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

                        // set playback started response
                        output = SsmlTemplate.PlayAudioSpeech(currentMediaItem.Title);
                        response = ResponseBuilder.Tell(output);
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
