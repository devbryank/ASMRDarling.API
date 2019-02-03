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

#warning styles need to be sorted out

        // Menu display layout generation start
        public static async Task<SkillResponse> MenuDisplay(SkillResponse response)
        {

            Style newStyle = new Style();

            StyleValue colorRed = new StyleValue();
            colorRed.Properties = new Dictionary<string, object> { { "color", "red" } };

            StyleValue bgRed = new StyleValue();
            bgRed.Properties = new Dictionary<string, object> { { "backgroundColor", "red" } };

            newStyle.Values = new List<StyleValue>();

            newStyle.Values.Add(colorRed);
            newStyle.Values.Add(bgRed);


            Style redStyle = new Style();





            var mainLayout = new Layout(new[] {
                new Frame(
                                new Container(
                                    new Sequence(
                                        new APLComponent[] {
                                           
                                            // Title
                                            new Text("ASMR List") { FontSize = "24dp", TextAlign = "center", Style = redStyle },

                                            // First clip
                                            new TouchWrapper(
                                                new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/10triggerstohelpyousleep.png") { Width = "40vw", Height = "30vh", Position = "center" },
                                                new Text(MediaItems.GetMediaItems().Find(m => m.Title.Contains("10 Triggers")).Title) { Color = "red", FontSize = "20dp", TextAlign = "center" }) {
                                                OnPress = new SendEvent {
                                                    Arguments = new List<string> { MediaItems.GetMediaItems().Find(m => m.Title.Contains("10 Triggers")).VideoSource }
                                                }
                                            },

                                            // Second clip
                                            new TouchWrapper(new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/20triggerstohelpyousleep.png")) {
                                                OnPress = new SendEvent {
                                                    Arguments = new List<string> { MediaItems.GetMediaItems().Find(m => m.Title.Contains("20 Triggers")).VideoSource }
                                                }
                                            },

                                            // Third clip
                                            new TouchWrapper(new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/100triggerstohelpyousleep.png")) {
                                                OnPress = new SendEvent {
                                                    Arguments = new List<string> { MediaItems.GetMediaItems().Find(m => m.Title.Contains("100 Triggers")).VideoSource }
                                                }
                                            },

                                            // Fourth clip
                                            new TouchWrapper(new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/atoztriggerstohelpyousleep.png")) {
                                                OnPress = new SendEvent {
                                                    Arguments = new List<string> { MediaItems.GetMediaItems().Find(m => m.Title.Contains("A to Z Triggers")).VideoSource }
                                                }
                                            },

                                            // Fifth clip
                                            new TouchWrapper(new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/brushingthemicrophone.png")) {
                                                OnPress = new SendEvent {
                                                    Arguments = new List<string> { MediaItems.GetMediaItems().Find(m => m.Title.Contains("Brushing")).VideoSource }
                                                }
                                            },

                                            // Sixth clip
                                            new TouchWrapper(new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/closeuppersonalattentionforyoutosleep.png")) {
                                                OnPress = new SendEvent {
                                                    Arguments = new List<string> { MediaItems.GetMediaItems().Find(m => m.Title.Contains("Personal")).VideoSource }
                                                }
                                            },

                                            // Seventh clip
                                            new TouchWrapper(new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/relaxingheadmassage.png")) {
                                                OnPress = new SendEvent {
                                                    Arguments = new List<string> { MediaItems.GetMediaItems().Find(m => m.Title.Contains("Head")).VideoSource }
                                                }
                                            },

                                            // Eighth clip
                                            new TouchWrapper(new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/relaxingscalpmassage.png")) {
                                                OnPress = new SendEvent {
                                                    Arguments = new List<string> { MediaItems.GetMediaItems().Find(m => m.Title.Contains("Scalp")).VideoSource }
                                                }
                                            },

                                            // Ninth clip
                                            new TouchWrapper(new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/whatisasmr.png")) {
                                                OnPress = new SendEvent {
                                                    Arguments = new List<string> { MediaItems.GetMediaItems().Find(m => m.Title.Contains("ASMR")).VideoSource }
                                                }
                                            },

                                            // Tenth clip
                                            new TouchWrapper(new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/whisperedtappingandscratching.png")) {
                                                OnPress = new SendEvent {
                                                    Arguments = new List<string> { MediaItems.GetMediaItems().Find(m => m.Title.Contains("Whispered")).VideoSource }
                                                }
                                            }

                                        } // End of APLComponent
                                    ) { Width = "100vw", Height = "100vh", AlignSelf = "center", Position = "center" } // End of Sequence
                                ) { Width = "100vw", Height = "100vh", Direction = "row", JustifyContent = "center" } ){ BackgroundColor = "white"}// End of Container
                            }); // End of Layout

            // Make a rendering response
            var renderDocument = new RenderDocumentDirective
            {
                Token = "APLMenuDisplay",
                Document = new APLDocument
                {
                    Styles = new Dictionary<string, Style> { { "newStyle", newStyle } },

                    MainTemplate = mainLayout
                }
            };

            // Merge APL directives to the response then return to the play media intent handler
            response.Response.Directives.Add(renderDocument);
            return response;
        }


        // Video player layout generation start
        public static async Task<SkillResponse> VideoPlayer(SkillResponse response, string source)
        {
            // Set video source
            List<VideoSource> videoList = new List<VideoSource> { new VideoSource(source) };

            var mainLayout = new Layout(new[] {
                                new Container(
                                    new Video(
                                    ) { Width = "100vw", Height = "100vh", Autoplay = true, Source = videoList } // End of Video
                                ) // End of Container
                            }); // End of Layout

            //// Make a rendering response
            var renderDocument = new RenderDocumentDirective
            {
                Token = "APLVideoPlayer",
                Document = new APLDocument { MainTemplate = mainLayout }
            };

            // Merge APL directives to the response then return to the play media intent handler
            response.Response.Directives.Add(renderDocument);
            return response;
        }
    }
}
