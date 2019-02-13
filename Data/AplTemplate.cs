using System.Threading.Tasks;
using System.Collections.Generic;
using Alexa.NET.Response;
using Alexa.NET.Response.APL;
using Alexa.NET.APL.Commands;
using Alexa.NET.APL.Components;
using ASMRDarling.API.Models;

namespace ASMRDarling.API.Data
{
#warning need to be updated

    /// <summary>
    /// 
    /// </summary>
    class AplTemplate
    {
        // image properties
        static string imageWidth = "50vw";
        static string imageHeight = "40vh";
        static string imagePaddingBottom = "5vh";

        // font properties
        static string mediaNameFontSize = "30dp";
        static string mediaNameFontColor = "black";


        // menu display layout generation start
        public static async Task<SkillResponse> MenuDisplay(SkillResponse response)
        {
            // generate a touch wrapped thumbnail array
            var thumbnails = GetTouchWrappedThumbnails(MediaItems.GetMediaItems());

            var mainLayout = new Layout(
                                new Frame(
                                    new Container(
                                        new Sequence(thumbnails) { Width = imageWidth, Height = "100vh" }
                                    )
                                    { Width = "100vw", Height = "100vh", AlignItems = "center", JustifyContent = "center" } // end of container
                                )
                                { Width = "100vw", Height = "100vh", BackgroundColor = "white" } // end of frame
                             ); // end of layout

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
        protected static IEnumerable<APLComponent> GetTouchWrappedThumbnails(List<MediaItem> mediaItemList)
        {
            TouchWrapper[] touchWrappers = new TouchWrapper[mediaItemList.Count];

            for (int i = 0; i < mediaItemList.Count; i++)
            {
                var mediaItem = mediaItemList[i];

                touchWrappers[i] = new TouchWrapper
                (
                    new Image(mediaItem.Thumbnail) { Width = imageWidth, Height = imageHeight },
                    new Text($"({ mediaItem.Id }) { mediaItem.Title }") { Color = mediaNameFontColor, FontSize = mediaNameFontSize, TextAlign = "center", PaddingBottom = imagePaddingBottom }
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
            }

            return touchWrappers;
        }
    }
}
