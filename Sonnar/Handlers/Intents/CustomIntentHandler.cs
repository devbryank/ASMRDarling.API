using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Alexa.NET.Response.Directive;

using Sonnar.Models;
using Sonnar.Helpers;
using Sonnar.Templates;
using Sonnar.Components;
using Sonnar.Interfaces;

namespace Sonnar.Handlers
{
    class CustomIntentHandler : IRequestHandler
    {
        public async Task HandleRequest()
        {
            await RequestProcessHelper.ProcessRequest("CustomIntentHandler.HandleRequest()", "Custom Intent", async () =>
            {
                IntentRequest request = Core.Request.GetRequest().Request as IntentRequest;

                // get slot resolution
                string slotValue;
                ProgressiveResponse progressiveResponse = new ProgressiveResponse(Core.Request.GetRequest());
                Slot slot = request.Intent.Slots[AlexaRequestType.CustomSlot];

                string rawValue = slot.Value;
                Core.Logger.Write("CustomIntentHandler.HandleRequest()", $"Sub intent raw slot value: {rawValue}");

                try
                {
                    ResolutionAuthority[] resolution = slot.Resolution.Authorities;
                    ResolutionValueContainer[] container = resolution[0].Values;
                    slotValue = container[0].Value.Name;

                    Core.Logger.Write("CustomIntentHandler.HandleRequest()", $"Sub intent processed slot value: {slotValue}");
                }
                catch
                {
                    slotValue = rawValue;
                }

                // if user says list
                if (slotValue.Equals("List"))
                {
                    Core.State.UserState.Stage = Stage.Menu;
                    Core.Response.SetSpeech(false, false, SpeechTemplate.ListItems);
                }

                // if user says random or shuffle
                else if (slotValue.Equals("Random") || (slotValue.Equals("Shuffle")))
                {
                    Random mediaRandom = new Random();
                    List<MediaItem> mediaItems = MediaItems.GetMediaItems();
                    MediaItem currentMediaItem = mediaItems[mediaRandom.Next(mediaItems.Count)];

                    // update database context
                    Core.State.UserState.Shuffle = true;
                    Core.State.UserState.OffsetInMS = 0;
                    Core.State.UserState.Index = currentMediaItem.Id;
                    Core.State.UserState.Token = currentMediaItem.FileName;

                    await progressiveResponse.SendSpeech($"Playing a random recording. {currentMediaItem.Title}. To call me back, say cancel. ");

                    if (Core.Device.HasScreen)
                    {
                        Core.State.UserState.Stage = Stage.Video;
                        Core.Response.AddVideoApp(currentMediaItem.VideoSource, currentMediaItem.Title, currentMediaItem.FileName);
                    }
                    else
                    {
                        Core.State.UserState.Stage = Stage.Audio;
                        Core.Response.AddAudioPlayer(PlayBehavior.ReplaceAll, currentMediaItem.AudioSource, currentMediaItem.FileName);
                    }
                }
                else
                {
                    if (slotValue != null && slotValue != string.Empty)
                    {
                        List<MediaItem> mediaItems = MediaItems.GetMediaItems();
                        List<MediaItem> selectedMedia = ItemSelectHelper.SelectItems(mediaItems, "Title", slotValue);

                        // query possible entries
                        if (selectedMedia.Any())
                        {
                            if (selectedMedia.Count > 1)
                            {
                                StringBuilder sb = new StringBuilder();
                                sb.Append($"I found {selectedMedia.Count} recordings matching {slotValue}. ");
                                Core.Logger.Write("CustomIntentHandler.HandleRequest()", $"{selectedMedia.Count} matching items found for {slotValue}");

                                foreach (MediaItem mediaItem in selectedMedia)
                                    sb.Append(mediaItem.Title + ". ");

                                sb.Append("Please choose one of the following. ");
                                Core.Response.SetSpeech(false, false, sb.ToString());
                            }
                            else
                            {
                                MediaItem mediaItem = selectedMedia[0];
                                Core.Logger.Write("CustomIntentHandler.HandleRequest()", $"Item requested: {mediaItem.Title}");

                                Core.State.UserState.Index = mediaItem.Id;
                                Core.State.UserState.Token = mediaItem.FileName;

                                // source type will differ based on the display interface
                                string url = Core.Device.HasScreen ? mediaItem.VideoSource : mediaItem.AudioSource;
                                Core.Logger.Write("CustomIntentHandler.HandleRequest()", $"File source url: {url}");

                                await progressiveResponse.SendSpeech($"Playing {mediaItem.Title}. ");

                                if (Core.Device.HasScreen)
                                {
                                    Core.State.UserState.Stage = Stage.Video;
                                    Core.Response.AddVideoApp(url, mediaItem.Title, mediaItem.FileName);
                                }
                                else
                                {
                                    Core.State.UserState.Stage = Stage.Audio;
                                    Core.Response.AddAudioPlayer(PlayBehavior.ReplaceAll, url, Core.State.UserState.Token);
                                }
                            }
                        }
                        else
                        {
                            Core.State.UserState.Stage = Stage.Menu;
                            Core.Response.SetSpeech(false, false, $"I could not find any recording matching {slotValue}. Try saying list, or shuffle. ");
                        }
                    }
                }
            });
        }
    }
}
