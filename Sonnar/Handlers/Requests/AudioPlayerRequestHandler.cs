using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Alexa.NET.Request.Type;
using Alexa.NET.Response.Directive;

using Sonnar.Models;
using Sonnar.Helpers;
using Sonnar.Components;
using Sonnar.Interfaces;

namespace Sonnar.Handlers
{
    class AudioPlayerRequestHandler : IRequestHandler
    {
        public async Task HandleRequest()
        {
            await RequestProcessHelper.ProcessRequest("AudioPlayerRequestHandler.HandleRequest()", "Audio Player Request", async () =>
            {
                AudioPlayerRequest request = Core.Request.GetRequest().Request as AudioPlayerRequest;
                List<MediaItem> mediaItems = MediaItems.GetMediaItems();
                MediaItem currentMediaItem = mediaItems.Find(m => m.Title.Contains(request.Token));

                switch (Core.Request.GetSubRequestType())
                {
                    // handle playback started
                    case AlexaRequests.PlaybackStarted:
                        Core.Response.ClearQueue();
                        break;

                    // handle playback stopped
                    case AlexaRequests.PlaybackStopped:
                        Core.State.UserState.State = "PAUSE_MODE";
                        Core.State.UserState.Token = request.Token;
                        Core.State.UserState.EnqueuedToken = request.EnqueuedToken;
                        Core.State.UserState.OffsetInMS = Convert.ToInt32(request.OffsetInMilliseconds);
                        break;

                    // handle playback nearly finished
                    case AlexaRequests.PlaybackNearlyFinished:
                        if (!request.HasEnqueuedItem)
                        {
                            var currentPlay = request.Token;
                            int index = mediaItems.IndexOf(mediaItems.Where(m => m.Title == request.Token).FirstOrDefault());

                            if (index == -1)
                                index++;

                            if (index == MediaItems.GetMediaItems().Count)
                                index = 0;

                            Core.State.UserState.EnqueuedToken = mediaItems[index].Title;
                            Core.State.UserState.State = request.Token;
                            Core.Response.AddAudioPlayer(PlayBehavior.Enqueue, mediaItems[index].AudioSource, Core.State.UserState.EnqueuedToken, Core.State.UserState.Token, 0);
                        }
                        break;

                    // handle playback finished
                    case AlexaRequests.PlaybackFinished:
                        if (Core.State.UserState.EnqueuedToken != null)
                        {
                            int index = mediaItems.IndexOf(mediaItems.Where(m => m.Title == Core.State.UserState.EnqueuedToken).FirstOrDefault());
                            Core.State.UserState.Token = mediaItems[index].Title;
                            Core.State.UserState.Index = index;
                            Core.Response.AddAudioPlayer(PlayBehavior.ReplaceAll, mediaItems[index].AudioSource, Core.State.UserState.Token);
                        }
                        else
                            Core.Response.ClearQueue();
                        break;

                    // handle playback failed
                    case AlexaRequests.PlaybackFailed:
                        Core.State.UserState.Token = mediaItems.FirstOrDefault().Title;
                        Core.State.UserState.Index = 0;
                        Core.State.UserState.State = "PLAY_MODE";
                        Core.Response.AddAudioPlayer(PlayBehavior.ReplaceAll, mediaItems.FirstOrDefault().AudioSource, Core.State.UserState.Token);
                        break;

                    // handle unknown request
                    default:
                        break;
                }
            });
        }
    }
}
