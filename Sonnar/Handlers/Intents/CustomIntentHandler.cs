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
                // get slot resolution
                string slotValue;
                IntentRequest request = Core.Request.GetRequest().Request as IntentRequest;
                Slot slot = request.Intent.Slots[AlexaRequests.CustomSlot];

                string rawValue = slot.Value;
                Core.Logger.Write("CustomIntentHandler.HandleRequest()", $"Sub intent raw slot value: {rawValue}");

                try
                {
                    ResolutionAuthority[] resolution = slot.Resolution.Authorities;
                    ResolutionValueContainer[] container = resolution[0].Values;
                    slotValue = container[0].Value.Name;
                }
                catch { slotValue = rawValue; }

                if (slotValue == ("List"))
                    Core.Response.SetAskSpeech(SpeechTemplate.ListItems);
                else
                {
                    if (slotValue != null && slotValue != string.Empty)
                    {
                        List<MediaItem> mediaItems = MediaItems.GetMediaItems();
                        List<MediaItem> selectedMedia = ItemSelectHelper.SelectItems(mediaItems, "Title", slotValue);

                        // query possible entries
                        if (selectedMedia.Count > 1)
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.Append($"I found {selectedMedia.Count} media items matching {slotValue}. ");
                            Core.Logger.Write("CustomIntentHandler.HandleRequest()", $"{selectedMedia.Count} matching items found for {slotValue}");

                            foreach (MediaItem mediaItem in selectedMedia)
                                sb.Append(mediaItem.Title + ". ");

                            sb.Append("Please say play followed by one of the options. ");
                            Core.Response.SetAskSpeech(sb.ToString());
                        }
                        else if (selectedMedia.Count == 0)
                        {
                            Core.Response.SetAskSpeech(SpeechTemplate.NotUnderstand + SpeechTemplate.MoreOptions);
                        }
                        else
                        {
                            MediaItem mediaItem = selectedMedia[0];
                            Core.Logger.Write("CustomIntentHandler.HandleRequest()", $"Item requested: {mediaItem.Title}");

                            Core.State.UserState.Index = mediaItem.Id;
                            Core.State.UserState.Token = mediaItem.FileName;
                            Core.State.UserState.State = "PLAY_MODE";

                            // source type will differ based on the display interface
                            string url = Core.Device.HasScreen ? mediaItem.VideoSource : mediaItem.AudioSource;
                            Core.Logger.Write("CustomIntentHandler.HandleRequest()", $"File source url: {url}");

                            ProgressiveResponse progressiveResponse = new ProgressiveResponse(Core.Request.GetRequest());
                            await progressiveResponse.SendSpeech($"Playing {mediaItem.Title}. ");

                            if (Core.Device.HasScreen)
                                Core.Response.AddVideoApp(url, mediaItem.Title, mediaItem.FileName);
                            else
                                Core.Response.AddAudioPlayer(PlayBehavior.ReplaceAll, url, Core.State.UserState.Token);
                        }
                    }
                }
            });
        }
    }
}
