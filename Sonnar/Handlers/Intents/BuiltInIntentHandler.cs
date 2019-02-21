using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Alexa.NET.Response;
using Alexa.NET.Response.Directive;

using Sonnar.Models;
using Sonnar.Helpers;
using Sonnar.Templates;
using Sonnar.Components;
using Sonnar.Interfaces;

namespace Sonnar.Handlers
{
    class BuiltInIntentHandler : IRequestHandler
    {
        public async Task HandleRequest()
        {
            await RequestProcessHelper.ProcessRequest("BuiltInIntentHandler.HandleRequest()", "Built In Intent", async () =>
            {
                int currentMediaIndex = Core.State.UserState.Index;
                string subIntentType = Core.Request.GetSubIntentType();

                List<MediaItem> mediaItems = MediaItems.GetMediaItems();
                MediaItem currentMediaItem = mediaItems.Find(m => m.Id.Equals(currentMediaIndex));
                ProgressiveResponse progressiveResponse = new ProgressiveResponse(Core.Request.GetRequest());

                // prevent user to enter if not on play mode, help and stop can be processed
                if (Core.State.UserState.State != "MENU_MODE" || subIntentType.Equals(AlexaRequests.BuiltInHelp) || subIntentType.Equals(AlexaRequests.BuiltInStop) || subIntentType.Equals(AlexaRequests.BuiltInRepeat))
                {
                    switch (subIntentType)
                    {
                        // handle help intent
                        case AlexaRequests.BuiltInHelp:
                            Core.State.UserState.State = "MENU_MODE";
                            Core.Response.SetAskSpeech(SpeechTemplate.MoreOptions);
                            Core.Logger.Write("BuiltInIntentHandler.HandleRequest()", $"User state updated to: {Core.State.UserState.State}");
                            break;

                        // handle next intent
                        case AlexaRequests.BuiltInNext:
                            MediaItem nextMediaItem = mediaItems.Find(m => m.Id.Equals(currentMediaIndex + 1));

                            if (nextMediaItem != null)
                            {
                                await progressiveResponse.SendSpeech($"Playing next media item, {nextMediaItem.Title}. ");
                                Core.Logger.Write("BuiltInIntentHandler.HandleRequest()", $"Next media item name: {nextMediaItem.Title}");
                                Core.State.UserState.State = "PLAY_MODE";

                                Core.State.UserState.Index = nextMediaItem.Id;
                                Core.State.UserState.Token = nextMediaItem.FileName;
                                Core.State.UserState.OffsetInMS = 0;

                                Core.Response.AddAudioPlayer(PlayBehavior.ReplaceAll, nextMediaItem.AudioSource, nextMediaItem.FileName);
                            }
                            else
                            {
                                Core.Logger.Write("BuiltInIntentHandler.HandleRequest()", "Next media item is not available");
                                Core.Response.SetAskSpeech(SpeechTemplate.NoNext);
                            }
                            break;

                        // handle previous intent
                        case AlexaRequests.BuiltInPrevious:
                            MediaItem previousMediaItem = mediaItems.Find(m => m.Id.Equals(currentMediaIndex - 1));
                            Core.State.UserState.State = "PLAY_MODE";
                            if (!Core.Device.HasScreen)
                            {
                                if (previousMediaItem != null)
                                {
                                    await progressiveResponse.SendSpeech($"Playing previous media item, {previousMediaItem.Title}. ");
                                    Core.Logger.Write("BuiltInIntentHandler.HandleRequest()", $"Previous media item name: {previousMediaItem.Title}");

                                    Core.State.UserState.Index = previousMediaItem.Id;
                                    Core.State.UserState.Token = previousMediaItem.FileName;
                                    Core.State.UserState.OffsetInMS = 0;

                                    Core.Response.AddAudioPlayer(PlayBehavior.ReplaceAll, previousMediaItem.AudioSource, previousMediaItem.FileName);
                                }
                                else
                                {
                                    Core.Logger.Write("BuiltInIntentHandler.HandleRequest()", "Previous media item is not available");
                                    Core.Response.SetAskSpeech(SpeechTemplate.NoPrevious);
                                }
                            }
                            else
                            {
                                Core.Response.SetAskSpeech(SpeechTemplate.NotUnderstand + SpeechTemplate.MoreOptions);
                            }
                            break;

                        // handle resume intent
                        case AlexaRequests.BuiltInResume:
                            Core.Response.AddAudioPlayer(PlayBehavior.ReplaceAll, currentMediaItem.AudioSource, Core.State.UserState.Token, Core.State.UserState.OffsetInMS);
                            Core.State.UserState.State = "PLAY_MODE";

                            if (Core.State.UserState.EnqueuedToken != null)
                            {
                                Core.Response.AddDirective(new AudioPlayerPlayDirective()
                                {
                                    PlayBehavior = PlayBehavior.Enqueue,
                                    AudioItem = new AudioItem()
                                    {
                                        Stream = new AudioItemStream()
                                        {
                                            Url = mediaItems[currentMediaIndex + 1].AudioSource,
                                            Token = mediaItems[currentMediaIndex + 1].Title,
                                            ExpectedPreviousToken = Core.State.UserState.Token,
                                            OffsetInMilliseconds = 0
                                        }
                                    }
                                });
                            }
                            break;

                        // handle cancel intent
                        case AlexaRequests.BuiltInCancel:
                            Core.State.UserState.OffsetInMS = Convert.ToInt32(Core.Request.GetRequest().Context.AudioPlayer.OffsetInMilliseconds);
                            Core.State.UserState.Token = Core.Request.GetRequest().Context.AudioPlayer.Token;
                            Core.State.UserState.State = "PAUSE_MODE";
                            Core.Response.StopAudioPlayer();
                            break;

                        // handle pause intent
                        case AlexaRequests.BuiltInPause:
                            await progressiveResponse.SendSpeech("See you soon. ");
                            Core.State.UserState.OffsetInMS = Convert.ToInt32(Core.Request.GetRequest().Context.AudioPlayer.OffsetInMilliseconds);
                            Core.State.UserState.OffsetInMS = Convert.ToInt32(Core.Request.GetRequest().Context.AudioPlayer.OffsetInMilliseconds);
                            Core.State.UserState.Token = Core.Request.GetRequest().Context.AudioPlayer.Token;
                            Core.State.UserState.State = "PAUSE_MODE";
                            Core.Response.SetTellSpeech(SpeechTemplate.SeeYouSoon);
                            Core.Response.StopAudioPlayer();
                            break;

                        // handle stop intent
                        case AlexaRequests.BuiltInStop:
                            try
                            {
                                string activity = SessionHelper.Get<string>("audio_activity");
                                if (!activity.Equals("IDLE"))
                                    Core.Response.StopAudioPlayer();
                            }
                            catch { }

                            Core.Response.SetTellSpeech(SpeechTemplate.SeeYouSoon);
                            Core.State.UserState.State = "PAUSE_MODE";
                            break;

                        // handle repeat intent
                        case AlexaRequests.BuiltInRepeat:
                            if (Core.State.UserState.State.Equals("MENU_MODE"))
                            {
                                Core.Response.SetAskSpeech(SpeechTemplate.Intro);
                            }
                            else if (Core.State.UserState.State.Equals("PLAY_MODE") || Core.State.UserState.State.Equals("PAUSE_MODE"))
                            {
                                Core.State.UserState.Token = currentMediaItem.Title;
                                Core.State.UserState.OffsetInMS = 0;
                                Core.State.UserState.State = "PLAY_MODE";
                                Core.Response.AddAudioPlayer(PlayBehavior.ReplaceAll, currentMediaItem.AudioSource, currentMediaItem.Title, 0);
                            }
                            break;

                        // handle loop on intent
                        case AlexaRequests.BuiltInLoopOn:
                            await progressiveResponse.SendSpeech("Loop mode on. ");
                            Core.State.UserState.Loop = true;
                            break;

                        // handle loop off intent
                        case AlexaRequests.BuiltInLoopOff:
                            await progressiveResponse.SendSpeech("Loop mode off. ");
                            Core.State.UserState.Loop = false;
                            break;

                        // handle shuffle on intent
                        case AlexaRequests.BuiltInShuffleOn:
                            await progressiveResponse.SendSpeech("Shuffle mode on. ");
                            Core.State.UserState.Shuffle = true;
                            break;

                        // handle shuffle off intent
                        case AlexaRequests.BuiltInShuffleOff:
                            await progressiveResponse.SendSpeech("Shuffle mode off. ");
                            Core.State.UserState.Shuffle = false;
                            break;

                        // handle start over intent
                        case AlexaRequests.BuiltInStartOver:
                            Core.State.UserState.Token = mediaItems[0].Title;
                            Core.State.UserState.OffsetInMS = 0;
                            Core.State.UserState.State = "PLAY_MODE";
                            Core.Response.AddAudioPlayer(PlayBehavior.ReplaceAll, mediaItems[0].AudioSource, mediaItems[0].Title, 0);
                            break;

                        // handle fallback intent
                        case AlexaRequests.BuiltInFallback:
                            Core.Response.SetTellSpeech(SpeechTemplate.IntentUnknown);
                            break;

                        // handle unknown intent
                        default:
                            Core.Response.SetTellSpeech(SpeechTemplate.IntentUnknown);
                            break;
                    }
                }
                else
                {
                    Core.Logger.Write("BuiltInIntentHandler.HandleRequest()", $"Unable to process {subIntentType}, user is at {Core.State.UserState.State}");
                    Core.Response.SetAskSpeech(SpeechTemplate.NoCommand);
                }
            });
        }
    }
}
