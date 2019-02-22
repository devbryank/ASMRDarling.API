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
                // get the most recent media index & sub intent
                int currentMediaIndex = Core.State.UserState.Index;
                string subIntentType = Core.Request.GetSubIntentType();

                Core.Logger.Write("BuiltInIntentHandler.HandleRequest()", $"Current media index: {currentMediaIndex}");
                Core.Logger.Write("BuiltInIntentHandler.HandleRequest()", $"User is at: {Core.State.UserState.Stage}");


                // get media item components
                List<MediaItem> mediaItems = MediaItems.GetMediaItems();
                MediaItem currentMediaItem = mediaItems[currentMediaIndex];


                // set response components
                Random random = new Random();
                ProgressiveResponse progressiveResponse = new ProgressiveResponse(Core.Request.GetRequest());
                List<string> stopSpeeches = new List<string> { SpeechTemplate.GoodBye, SpeechTemplate.SeeYouSoon, SpeechTemplate.SeeYouNextTime };


                // direct sub intent into a matching handler
                switch (subIntentType)
                {

#warning check if help is accessible from audioplayer and what happens
                    // handle help intent
                    case AlexaRequestType.BuiltInHelp:
                        Core.State.UserState.Stage = Stage.Menu;
                        Core.Response.SetSpeech(false, false, SpeechTemplate.Help);
                        break;

                    // handle stop intent
                    case AlexaRequestType.BuiltInStop:
                        Core.State.UserState.Stage = Stage.Menu; ;
                        Core.Response.StopAudioPlayer();
                        Core.Response.SetSpeech(false, true, stopSpeeches[random.Next(stopSpeeches.Count)]);
                        break;

                    // handle pause intent
                    case AlexaRequestType.BuiltInPause:
                        Core.State.UserState.Stage = Stage.Menu;
                        Core.State.UserState.Token = Core.Request.GetRequest().Context.AudioPlayer.Token;
                        Core.State.UserState.OffsetInMS = Convert.ToInt32(Core.Request.GetRequest().Context.AudioPlayer.OffsetInMilliseconds);

                        Core.Response.StopAudioPlayer();
                        Core.Response.SetSpeech(false, true, stopSpeeches[random.Next(stopSpeeches.Count)]);
                        break;

                    // handle cancel intent
                    case AlexaRequestType.BuiltInCancel:
                        if (Core.State.UserState.Stage.Equals(Stage.Audio))
                        {
                            Core.State.UserState.Stage = Stage.Menu;
                            Core.State.UserState.Token = Core.Request.GetRequest().Context.AudioPlayer.Token;
                            Core.State.UserState.OffsetInMS = Convert.ToInt32(Core.Request.GetRequest().Context.AudioPlayer.OffsetInMilliseconds);

                            Core.Response.StopAudioPlayer();
                            Core.Response.SetSpeech(false, false, SpeechTemplate.Intro);
                        }
                        else
                            Core.Response.SetSpeech(false, false, SpeechTemplate.NoUnderstand);
                        break;

                    // handle next intent
                    case AlexaRequestType.BuiltInNext:
                        if (Core.State.UserState.Stage.Equals(Stage.Audio))
                        {
                            MediaItem nextMediaItem = mediaItems.Find(m => m.Id.Equals(currentMediaIndex + 1));

                            if (nextMediaItem != null)
                            {
                                await progressiveResponse.SendSpeech($"Playing next recording, {nextMediaItem.Title}. ");
                                Core.Logger.Write("BuiltInIntentHandler.HandleRequest()", $"Next media item name: {nextMediaItem.Title}");

                                Core.State.UserState.OffsetInMS = 0;
                                Core.State.UserState.Stage = Stage.Audio;
                                Core.State.UserState.Index = nextMediaItem.Id;
                                Core.State.UserState.Token = nextMediaItem.FileName;

                                Core.Response.AddAudioPlayer(PlayBehavior.ReplaceAll, nextMediaItem.AudioSource, nextMediaItem.FileName);
                            }
                            else
                            {
                                Core.Logger.Write("BuiltInIntentHandler.HandleRequest()", "Next media item is not available");
                                Core.Response.SetSpeech(false, false, SpeechTemplate.NoNext);
                                Core.State.UserState.Stage = Stage.Pause;
                            }
                        }
                        else
                            Core.Response.SetSpeech(false, false, SpeechTemplate.NoUnderstand);
                        break;

                    // handle previous intent
                    case AlexaRequestType.BuiltInPrevious:
                        if (Core.State.UserState.Stage.Equals(Stage.Audio))
                        {
                            MediaItem previousMediaItem = mediaItems.Find(m => m.Id.Equals(currentMediaIndex - 1));

                            if (previousMediaItem != null)
                            {
                                await progressiveResponse.SendSpeech($"Playing previous recording, {previousMediaItem.Title}. ");
                                Core.Logger.Write("BuiltInIntentHandler.HandleRequest()", $"Previous media item name: {previousMediaItem.Title}");

                                Core.State.UserState.OffsetInMS = 0;
                                Core.State.UserState.Stage = Stage.Audio;
                                Core.State.UserState.Index = previousMediaItem.Id;
                                Core.State.UserState.Token = previousMediaItem.FileName;

                                Core.Response.AddAudioPlayer(PlayBehavior.ReplaceAll, previousMediaItem.AudioSource, previousMediaItem.FileName);
                            }
                            else
                            {
                                Core.Logger.Write("BuiltInIntentHandler.HandleRequest()", "Previous media item is not available");
                                Core.Response.SetSpeech(false, false, SpeechTemplate.NoPrevious);
                                Core.State.UserState.Stage = Stage.Pause;
                            }
                        }
                        else
                            Core.Response.SetSpeech(false, false, SpeechTemplate.NoUnderstand);
                        break;

                    // handle start over intent
                    case AlexaRequestType.BuiltInStartOver:
                        if (Core.State.UserState.Stage.Equals(Stage.Audio))
                        {
                            await progressiveResponse.SendSpeech($"Playing from the first recording. {mediaItems[0].Title}. ");
                            Core.State.UserState.OffsetInMS = 0;
                            Core.State.UserState.Stage = Stage.Audio;
                            Core.State.UserState.Token = mediaItems[0].Title;
                            Core.Response.AddAudioPlayer(PlayBehavior.ReplaceAll, mediaItems[0].AudioSource, mediaItems[0].Title, 0);
                        }
                        else
                            Core.Response.SetSpeech(false, false, SpeechTemplate.NoUnderstand);
                        break;

                    // handle resume intent
                    case AlexaRequestType.BuiltInResume:
                        if (Core.State.UserState.Stage.Equals(Stage.Pause))
                        {
                            Core.State.UserState.Stage = Stage.Audio;
                            Core.Response.AddAudioPlayer(PlayBehavior.ReplaceAll, currentMediaItem.AudioSource, Core.State.UserState.Token, Core.State.UserState.OffsetInMS);

                            if (Core.State.UserState.EnqueuedToken != null)
                            {
                                Core.Response.AddDirective(new AudioPlayerPlayDirective()
                                {
                                    PlayBehavior = PlayBehavior.Enqueue,
                                    AudioItem = new AudioItem()
                                    {
                                        Stream = new AudioItemStream()
                                        {
                                            OffsetInMilliseconds = 0,
                                            Url = mediaItems[currentMediaIndex + 1].AudioSource,
                                            Token = mediaItems[currentMediaIndex + 1].Title,
                                            ExpectedPreviousToken = Core.State.UserState.Token
                                        }
                                    }
                                });
                            }
                        }
                        else
                            Core.Response.SetSpeech(false, false, SpeechTemplate.NoUnderstand);
                        break;

                    // handle repeat intent
                    case AlexaRequestType.BuiltInRepeat:
                        if (Core.State.UserState.Stage.Equals(Stage.Audio))
                        {
                            await progressiveResponse.SendSpeech($"Repeating {currentMediaItem.Title}");
                            Core.State.UserState.OffsetInMS = 0;
                            Core.State.UserState.Stage = Stage.Audio;
                            Core.State.UserState.Token = currentMediaItem.Title;
                            Core.Response.AddAudioPlayer(PlayBehavior.ReplaceAll, currentMediaItem.AudioSource, currentMediaItem.Title, 0);
                        }
                        else
                            Core.Response.SetSpeech(false, false, SpeechTemplate.Intro);
                        break;

                    // handle unknown intent
                    default:
                        bool endSession = Core.State.UserState.NumReprompt > 4 ? true : false;
                        Core.Logger.Write("BuiltInIntentHandler.HandleRequest()", "Intent was not recognized, directing into the default case handler");
                        Core.Response.SetSpeech(false, endSession, SpeechTemplate.NoUnderstand);
                        Core.State.UserState.Stage = Stage.Menu;
                        Core.State.UserState.NumReprompt++;
                        if (endSession)
                            Core.State.UserState.NumReprompt = 0;
                        break;
                }
            });
        }
    }
}