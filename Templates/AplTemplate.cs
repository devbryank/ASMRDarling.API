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
        // Menu display layout
        public static async Task<SkillResponse> MenuDisplay(SkillResponse response)
        {
            var mainLayout = new Layout(new[] {
                                new Container(
                                    new Sequence(
                                        new APLComponent[] {
                                            
                                            // Title
                                            new Text("ASMR List") { FontSize = "24dp", TextAlign = "center" },

                                            // First clip
                                            new TouchWrapper(
                                                new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/10triggerstohelpyousleep.png"),
                                                new Text(MediaItems.GetMediaItems().Find(m => m.Title.Contains("10 Triggers")).Title) { TextAlign = "center" }) {
                                                OnPress = new SendEvent {
                                                    Arguments = new List<string> { MediaItems.GetMediaItems().Find(m => m.Title.Contains("10 Triggers")).VideoSource }
                                                }
                                            },

                                            // Second clip
                                            new TouchWrapper(new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/20triggerstohelpyousleep.png")) {
                                                OnPress = new SendEvent {
                                                    Arguments = new List<string> { MediaItems.GetMediaItems().Find(m=>m.Title.Contains("20 Triggers")).VideoSource }
                                                }
                                            },

                                            // Third clip
                                            new TouchWrapper(new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/100triggerstohelpyousleep.png")) {
                                                OnPress = new SendEvent {
                                                    Arguments = new List<string> { MediaItems.GetMediaItems().Find(m=>m.Title.Contains("100 Triggers")).VideoSource }
                                                }
                                            },

                                            // Fourth clip
                                            new TouchWrapper(new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/atoztriggerstohelpyousleep.png")) {
                                                OnPress = new SendEvent {
                                                    Arguments = new List<string> { MediaItems.GetMediaItems().Find(m=>m.Title.Contains("A to Z Triggers")).VideoSource }
                                                }
                                            },

                                            // Fifth clip
                                            new TouchWrapper(new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/brushingthemicrophone.png")) {
                                                OnPress = new SendEvent {
                                                    Arguments = new List<string> { MediaItems.GetMediaItems().Find(m=>m.Title.Contains("Brushing")).VideoSource }
                                                }
                                            },

                                            // Sixth clip
                                            new TouchWrapper(new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/closeuppersonalattentionforyoutosleep.png")) {
                                                OnPress = new SendEvent {
                                                    Arguments = new List<string> { MediaItems.GetMediaItems().Find(m=>m.Title.Contains("Personal")).VideoSource }
                                                }
                                            },

                                            // Seventh clip
                                            new TouchWrapper(new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/relaxingheadmassage.png")) {
                                                OnPress = new SendEvent {
                                                    Arguments = new List<string> { MediaItems.GetMediaItems().Find(m=>m.Title.Contains("Head")).VideoSource }
                                                }
                                            },

                                            // Eighth clip
                                            new TouchWrapper(new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/relaxingscalpmassage.png")) {
                                                OnPress = new SendEvent {
                                                    Arguments = new List<string> { MediaItems.GetMediaItems().Find(m=>m.Title.Contains("Scalp")).VideoSource }
                                                }
                                            },

                                            // Ninth clip
                                            new TouchWrapper(new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/whatisasmr.png")) {
                                                OnPress = new SendEvent {
                                                    Arguments = new List<string> { MediaItems.GetMediaItems().Find(m=>m.Title.Contains("ASMR")).VideoSource }
                                                }
                                            },

                                            // Tenth clip
                                            new TouchWrapper(new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/whisperedtappingandscratching.png")) {
                                                OnPress = new SendEvent {
                                                    Arguments = new List<string> { MediaItems.GetMediaItems().Find(m=>m.Title.Contains("Whispered")).VideoSource }
                                                }
                                            }

                                        } // End of APLComponent
                                    ) { Width = "40%", Height = "100vh" } // End of Sequence
                                ) { Direction = "row" } // End of Container
                            }); // End of Layout

            // Make a rendering response
            var renderDocument = new RenderDocumentDirective
            {
                Token = "APLMenuDisplay",
                Document = new APLDocument { MainTemplate = mainLayout }
            };

            // Merge APL directives to the response
            response.Response.Directives.Add(renderDocument);
            return response;
        }
    }
}
