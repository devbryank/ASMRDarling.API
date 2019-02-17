using System.Threading.Tasks;
using System.Collections.Generic;
using Alexa.NET.Response;
using Alexa.NET.Response.APL;
using Alexa.NET.APL.Commands;
using Alexa.NET.APL.Components;
using ASMRDarling.API.Models;
using ASMRDarling.API.Helpers;
using Amazon.Lambda.Core;

namespace ASMRDarling.API.Data
{
    /// <summary>
    /// 
    /// </summary>
    class AplTemplate
    {
        public static async Task<SkillResponse> MenuDisplay(SkillResponse response, ILambdaLogger logger)
        {

            if (Skill.IsRound)
            {
                //AddSpotContents();
            }
            else
            {
                //AddShowContents();
            }


            string top = DisplayHelper.GetHeight(0.15f);
            string size = DisplayHelper.GetHeight(0.7f);
            string spacing = DisplayHelper.GetWidth(0.08f);


            // generate a touch wrapped thumbnail array
            List<MediaItem> mediaItems = MediaItems.GetMediaItems();
            logger.LogLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");

            int mediaItemsSize = mediaItems.Count;
            int containerSize = (mediaItemsSize % 3) == 0 ? mediaItemsSize / 3 : (mediaItemsSize / 3) + 1;

            Container[] containers = new Container[containerSize];




            logger.LogLine($"BBBBBBBBBBBBBBBBBBBBBBBBBBBBBB      {containers}");

            Sequence sequence = new Sequence
            {
                Width = DisplayHelper.GetWidth(1f),
                Height = DisplayHelper.GetHeight(1f),
                Position = "absolute",
                ScrollDirection = "horizontal"
            };


            for (int i = 0; i < containerSize; i++)
            {


                List<APLComponent> containerItems = new List<APLComponent>
                {
                    new Container()
                };

                containers[i] = new Container
                {
                    Width = DisplayHelper.GetWidth(1f),
                    Height = DisplayHelper.GetHeight(1f),
                    Direction = "column"
                };

                for (int j = 0; j < 3; j++)
                {
                    if ((j + i * 3) < mediaItems.Count)
                    {
                        var wrappedItem = GetTouchWrappedItem(mediaItems[j + i + i + i]);

                        containerItems.Add(wrappedItem);
                    }

                }
                containers[i].Items = containerItems;

            }

            List<APLComponent> sequenceItems = new List<APLComponent>();

            for (int i = 0; i < containers.Length; i++)
            {
                sequenceItems.Add(containers[i]);
            }

            sequence.Items = sequenceItems;

            var mainLayout = new Layout(
                                new Frame(
                                   sequence
                                )
                                { Width = DisplayHelper.GetWidth(1f), Height = DisplayHelper.GetHeight(1f), BackgroundColor = "black" }
                             );




            // make a rendering response
            var renderDocument = new RenderDocumentDirective
            {
                Token = "APLMenuDisplay",
                Document = new APLDocument { MainTemplate = mainLayout }
            };

            // merge apl directives to the response then return to the play media intent handler
            response.Response.Directives.Add(renderDocument);
            return response;
        }


        // video player layout generation start
        public static async Task<SkillResponse> VideoPlayer(SkillResponse response, string source)
        {
            string videoWidth = "100vw";
            string videoHeight = "100vh";

            // set video source
            List<VideoSource> videoList = new List<VideoSource> { new VideoSource(source) };

            var mainLayout = new Layout(
                                new Container(
                                    new Video() { Id = "apl_video_player", Width = videoWidth, Height = videoHeight, Autoplay = true, Source = videoList }
                                ) // end of container
                             ); // end of layout

            // make a rendering response
            var renderDocument = new RenderDocumentDirective
            {
                Token = "APLVideoPlayer",
                Document = new APLDocument { MainTemplate = mainLayout }
            };

            // merge apl directives to the response then return to the requested handler
            response.Response.Directives.Add(renderDocument);
            return response;
        }


        // get touch wrapped apl thumbnails
        protected static APLComponent GetTouchWrappedItem(MediaItem mediaItem)
        {
            TouchWrapper touchWrapper = new TouchWrapper(
                                            new Container(
                                                new Image(mediaItem.Thumbnail)
                                                { Width = DisplayHelper.GetWidth(0.1f, 0.1f), Height = DisplayHelper.GetHeight(0.1f, 0.1f), AlignSelf = "center" },
                                                new Text(mediaItem.Title)
                                                { Color = "white", FontSize = "24dp", TextAlign = "center", MaxLines = 1, Height = DisplayHelper.GetHeight(0.1f) }
                                            )
                                            { Width = DisplayHelper.GetWidth(0.5f), Height = DisplayHelper.GetHeight(0.25f), Direction = "column", AlignItems = "center", Spacing = DisplayHelper.GetWidth(0.1f) }
                                        )
            {
                OnPress = new SendEvent
                {
                    Arguments = new List<string> {
                                                        mediaItem.Title,
                                                        mediaItem.VideoSource
    }
                }
            };


            return touchWrapper;
        }
    }
}
