using Amazon.Lambda.Core;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;
using ASMRDarling.API.Interfaces;

namespace ASMRDarling.API.Handlers
{
    class AudioPlayerIntentHandler : IAudioPlayerIntentHandler
    {
        // Constructor
        public AudioPlayerIntentHandler() { }


        // Intent handler start
        public Task<SkillResponse> HandleIntent(Intent intent, Session session, ILambdaLogger logger)
        {
            logger.LogLine($"[AudioPlayerIntentHandler.HandleIntent()] Audio player intent handling started");

            return null;
        }

        //AudioPlayer.ClearQueue
        //AudioPlayer.PlaybackStarted   say playing filename
        //AudioPlayer.PlaybackStopped  say play paused
        //AudioPlayer.PlaybackFinished
        //AudioPlayer.PlaybackFailed   should add a fallback?
    }
}
