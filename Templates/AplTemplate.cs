using System.Threading.Tasks;
using System.Collections.Generic;
using Alexa.NET.Response;
using Alexa.NET.Response.APL;
using Alexa.NET.APL.Commands;
using Alexa.NET.APL.Components;
using ASMRDarling.API.Models;

namespace ASMRDarling.API.Templates
{
    public class AplTemplate
    {
        // menu display layout generation start
        public static async Task<SkillResponse> MenuDisplay(SkillResponse response)
        {
            string imageWidth = "50vw";
            string imageHeight = "40vh";
            string imagePaddingBottom = "5vh";

            string clipNameFontSize = "30dp";
            string clipNameFontColor = "black";


            var mainLayout = new Layout(
                                new Frame(
                                    new Container(
                                        new Sequence(
                                            new APLComponent[] {
                                                // title
                                                new Text("ASMR List") { Color = "blue", TextAlign = "center", PaddingBottom = imagePaddingBottom },

                                                // first clip: what is asmr
                                                new TouchWrapper(
                                                    new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/whatisasmr.png") { Width = imageWidth, Height = imageHeight },
                                                    new Text($"(01) {MediaItems.GetMediaItems()[0].Title}") { Color = clipNameFontColor, FontSize = clipNameFontSize, TextAlign = "center", PaddingBottom = imagePaddingBottom }
                                                )
                                                {
                                                    OnPress = new SendEvent {
                                                        Arguments = new List<string> {
                                                            MediaItems.GetMediaItems().Find(m => m.Title.Contains("ASMR")).Title,
                                                            MediaItems.GetMediaItems().Find(m => m.Title.Contains("ASMR")).VideoSource
                                                        }
                                                    }
                                                },

                                                // second clip: 10 triggers to help you sleep
                                                new TouchWrapper(
                                                    new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/10triggerstohelpyousleep.png") { Width = imageWidth, Height = imageHeight },
                                                    new Text($"(02) {MediaItems.GetMediaItems()[1].Title}") { Color = clipNameFontColor, FontSize = clipNameFontSize, TextAlign = "center", PaddingBottom = imagePaddingBottom }
                                                )
                                                {
                                                    OnPress = new SendEvent {
                                                        Arguments = new List<string> {
                                                            MediaItems.GetMediaItems().Find(m => m.Title.Contains("10 Triggers")).Title,
                                                            MediaItems.GetMediaItems().Find(m => m.Title.Contains("10 Triggers")).VideoSource
                                                        }
                                                    }
                                                },

                                                // third clip: 20 triggers to help you sleep
                                                new TouchWrapper(
                                                    new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/20triggerstohelpyousleep.png") { Width = imageWidth, Height = imageHeight },
                                                    new Text($"(03) {MediaItems.GetMediaItems()[2].Title}") { Color = clipNameFontColor, FontSize = clipNameFontSize, TextAlign = "center", PaddingBottom = imagePaddingBottom }
                                                )
                                                {
                                                    OnPress = new SendEvent {
                                                        Arguments = new List<string> {
                                                            MediaItems.GetMediaItems().Find(m => m.Title.Contains("20 Triggers")).Title,
                                                            MediaItems.GetMediaItems().Find(m => m.Title.Contains("20 Triggers")).VideoSource
                                                        }
                                                    }
                                                },

                                                // fourth clip: 100 triggers to help you sleep
                                                new TouchWrapper(
                                                    new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/100triggerstohelpyousleep.png") { Width = imageWidth, Height = imageHeight },
                                                    new Text($"(04) {MediaItems.GetMediaItems()[3].Title}") { Color = clipNameFontColor, FontSize = clipNameFontSize, TextAlign = "center", PaddingBottom = imagePaddingBottom }
                                                )
                                                {
                                                    OnPress = new SendEvent {
                                                        Arguments = new List<string> {
                                                            MediaItems.GetMediaItems().Find(m => m.Title.Contains("100 Triggers")).Title,
                                                            MediaItems.GetMediaItems().Find(m => m.Title.Contains("100 Triggers")).VideoSource
                                                        }
                                                    }
                                                },

                                                // fifth clip: a to z triggers to help you sleep
                                                new TouchWrapper(
                                                    new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/atoztriggerstohelpyousleep.png") { Width = imageWidth, Height = imageHeight },
                                                    new Text($"(05) {MediaItems.GetMediaItems()[4].Title}") { Color = clipNameFontColor, FontSize = clipNameFontSize, TextAlign = "center", PaddingBottom = imagePaddingBottom }
                                                )
                                                {
                                                    OnPress = new SendEvent {
                                                        Arguments = new List<string> {
                                                            MediaItems.GetMediaItems().Find(m => m.Title.Contains("A to Z Triggers")).Title,
                                                            MediaItems.GetMediaItems().Find(m => m.Title.Contains("A to Z Triggers")).VideoSource
                                                        }
                                                    }
                                                },

                                                // sixth clip: brushing the microphone
                                                new TouchWrapper(
                                                    new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/brushingthemicrophone.png") { Width = imageWidth, Height = imageHeight },
                                                    new Text($"(06) {MediaItems.GetMediaItems()[5].Title}") { Color = clipNameFontColor, FontSize = clipNameFontSize, TextAlign = "center", PaddingBottom = imagePaddingBottom }
                                                )
                                                {
                                                    OnPress = new SendEvent {
                                                        Arguments = new List<string> {
                                                            MediaItems.GetMediaItems().Find(m => m.Title.Contains("Brushing")).Title,
                                                            MediaItems.GetMediaItems().Find(m => m.Title.Contains("Brushing")).VideoSource
                                                        }
                                                    }
                                                },

                                                // seventh clip: relaxing head massage
                                                new TouchWrapper(
                                                    new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/relaxingheadmassage.png") { Width = imageWidth, Height = imageHeight },
                                                    new Text($"(07) {MediaItems.GetMediaItems()[6].Title}") { Color = clipNameFontColor, FontSize = clipNameFontSize, TextAlign = "center", PaddingBottom = imagePaddingBottom }
                                                )
                                                {
                                                    OnPress = new SendEvent {
                                                        Arguments = new List<string> {
                                                            MediaItems.GetMediaItems().Find(m => m.Title.Contains("Head")).Title,
                                                            MediaItems.GetMediaItems().Find(m => m.Title.Contains("Head")).VideoSource
                                                        }
                                                    }
                                                },

                                                // eighth clip: relaxing scalp massage
                                                new TouchWrapper(
                                                    new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/relaxingscalpmassage.png") { Width = imageWidth, Height = imageHeight },
                                                    new Text($"(08) {MediaItems.GetMediaItems()[7].Title}") { Color = clipNameFontColor, FontSize = clipNameFontSize, TextAlign = "center", PaddingBottom = imagePaddingBottom }
                                                )
                                                {
                                                    OnPress = new SendEvent {
                                                        Arguments = new List<string> {
                                                            MediaItems.GetMediaItems().Find(m => m.Title.Contains("Scalp")).Title,
                                                            MediaItems.GetMediaItems().Find(m => m.Title.Contains("Scalp")).VideoSource
                                                        }
                                                    }
                                                },

                                                // ninth clip: whispered tapping and scratching
                                                new TouchWrapper(
                                                    new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/whisperedtappingandscratching.png") { Width = imageWidth, Height = imageHeight },
                                                    new Text($"(09) {MediaItems.GetMediaItems()[8].Title}") { Color = clipNameFontColor, FontSize = clipNameFontSize, TextAlign = "center", PaddingBottom = imagePaddingBottom }
                                                ) {
                                                    OnPress = new SendEvent {
                                                        Arguments = new List<string> {
                                                            MediaItems.GetMediaItems().Find(m => m.Title.Contains("Whispered")).Title,
                                                            MediaItems.GetMediaItems().Find(m => m.Title.Contains("Whispered")).VideoSource
                                                        }
                                                    }
                                                },

                                                // tenth clip: close up personal attention for you to sleep
                                                new TouchWrapper(
                                                    new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/closeuppersonalattentionforyoutosleep.png") { Width = imageWidth, Height = imageHeight },
                                                    new Text($"(10) {MediaItems.GetMediaItems()[9].Title}") { Color = clipNameFontColor, FontSize = clipNameFontSize, TextAlign = "center", PaddingBottom = imagePaddingBottom }
                                                )
                                                {
                                                    OnPress = new SendEvent {
                                                        Arguments = new List<string> {
                                                            MediaItems.GetMediaItems().Find(m => m.Title.Contains("Personal")).Title,
                                                            MediaItems.GetMediaItems().Find(m => m.Title.Contains("Personal")).VideoSource
                                                        }
                                                    }
                                                },
                                            } // end of apl component
                                        ) { Width = imageWidth, Height = "100vh" } // end of sequence
                                    ) { Width = "100vw", Height = "100vh", AlignItems = "center", JustifyContent = "center" } // end of container
                                ) { Width = "100vw", Height = "100vh", BackgroundColor = "white" } // end of frame
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
                                    new Video() { Width = videoWidth, Height = videoHeight, Autoplay = false, Source = videoList } // end of video
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
    }
}
