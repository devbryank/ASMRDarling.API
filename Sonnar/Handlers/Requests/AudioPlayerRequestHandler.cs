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
                    case AlexaRequestType.PlaybackStarted:
                        Core.Response.ClearAudioPlayer();
                        break;

                    // handle playback stopped
                    case AlexaRequestType.PlaybackStopped:
                        Core.State.UserState.Token = request.Token;
                        Core.State.UserState.EnqueuedToken = request.EnqueuedToken;
                        Core.State.UserState.OffsetInMS = Convert.ToInt32(request.OffsetInMilliseconds);
                        break;

                    // handle playback nearly finished
                    case AlexaRequestType.PlaybackNearlyFinished:
                        if (!request.HasEnqueuedItem)
                        {
                            var currentPlay = request.Token;
                            int index = mediaItems.IndexOf(mediaItems.Where(m => m.Title == request.Token).FirstOrDefault());

                            if (index == -1)
                                index++;

                            if (index == MediaItems.GetMediaItems().Count)
                                index = 0;

                            index = index == 0 ? 0 : index + 1;

                            Core.State.UserState.Token = request.Token;
                            Core.State.UserState.EnqueuedToken = mediaItems[index].Title;
                            Core.Response.AddAudioPlayer(PlayBehavior.Enqueue, mediaItems[index].AudioSource, Core.State.UserState.EnqueuedToken, Core.State.UserState.Token, 0);
                        }
                        break;

                    // handle playback finished
                    case AlexaRequestType.PlaybackFinished:
                        if (Core.State.UserState.EnqueuedToken != null)
                        {
                            int index = mediaItems.IndexOf(mediaItems.Where(m => m.Title == Core.State.UserState.EnqueuedToken).FirstOrDefault());
                            Core.State.UserState.Index = index;
                            Core.State.UserState.Token = mediaItems[index].Title;
                            Core.Response.AddAudioPlayer(PlayBehavior.ReplaceAll, mediaItems[index].AudioSource, Core.State.UserState.Token);
                        }
                        else
                            Core.Response.ClearAudioPlayer();
                        break;

                    // handle playback failed
                    case AlexaRequestType.PlaybackFailed:
                        Core.State.UserState.Index = 0;
                        Core.State.UserState.Stage = Stage.Menu;
                        Core.State.UserState.Token = mediaItems.FirstOrDefault().Title;
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
