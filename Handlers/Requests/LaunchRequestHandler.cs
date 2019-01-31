using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Amazon.Lambda.Core;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Alexa.NET.APL.Components;
using ASMRDarling.API.Builders;
using ASMRDarling.API.Interfaces;

namespace ASMRDarling.API.Handlers
{
    class LaunchRequestHandler : ILaunchRequestHandler
    {
        // Constructor
        public LaunchRequestHandler() { }


        // Request handler
        public async Task<SkillResponse> HandleRequest(LaunchRequest request, Session session, ILambdaLogger logger)
        {
            logger.LogLine($"[LaunchRequestHandler.HandleRequest()] Launch request handling started");

            // Get session display value
            bool? hasDisplay = session.Attributes["has_display"] as bool?;

            // If the device has display
            if (hasDisplay == true)
            {
                logger.LogLine($"[LaunchRequestHandler.HandleRequest()] Generating APL response");

                var output = SsmlBuilder.LaunchSpeech(hasDisplay);


                // working
                //Style style = new Style();
                //StyleValue styleValue = new StyleValue();
                //styleValue.Properties = new Dictionary<string, object>();
                //styleValue.Properties.Add("backgroundColor", "white");
                //styleValue.Properties.Add("color", "red");

                //style.Values = new List<StyleValue>() {

                //    styleValue

                //};


                // not working

                //IList<StyleValue> styleList = new List<StyleValue>();
                //IDictionary<string, string> dict = new Dictionary<string, string>
                //{
                //    { "backgroundColor", "white" }
                //};
                //styleList.Add(new StyleValue(dict));
                //style.Values = styleList;


                // if display is available
                List<VideoSource> videoList = new List<VideoSource> {
                    new VideoSource("https://s3.amazonaws.com/asmr-darling-api-media/mp4/100triggerstohelpyousleep.mp4")
                };


                // APL display to return
                //var mainLayout =
                //        new Layout(
                //            new[] {
                //                new Container (
                //                    new Video(
                //                    ) {Width = "80%", Source = videoList, Autoplay=true}, // End of Video
                //                    new Sequence(
                //                        new APLComponent[] {
                //                            // List of thumbnails
                //                            new Text("Video List") {FontSize = "24dp", TextAlign = "center"},
                //                            new TouchWrapper(
                //                            new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/10triggerstohelpyousleep.png") {Width = 200, Height = 150}){
                //                                OnPress = new SendEvent{

                //                                    Arguments = null
                //                                }
                //                            },
                //                            new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/20triggerstohelpyousleep.png") {Width = 200, Height = 150},
                //                            new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/100triggerstohelpyousleep.png") {Width = 200, Height = 150},
                //                            new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/atoztriggerstohelpyousleep.png") {Width = 200, Height = 150},
                //                            new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/brushingthemicrophone.png") {Width = 200, Height = 150},
                //                            new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/closeuppersonalattentionforyoutosleep.png") {Width = 200, Height = 150},
                //                            new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/relaxingheadmassage.png") {Width = 200, Height = 150},
                //                            new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/relaxingscalpmassage.png") {Width = 200, Height = 150},
                //                            new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/whatisasmr.png") {Width = 200, Height = 150},
                //                            new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/whisperedtappingandscratching.png") {Width = 200, Height = 150},
                //                        } // End of APLComponent
                //                    ) {Width = "20%", Height = "100vh", AlignSelf = "end", Style=style} // End of Sequence
                //                ) {Direction = "row", Style=style} // End of Container
                //            } // End of array
                //        ); // End of Layout

                //// Make a rendering response
                //var renderDocument = new RenderDocumentDirective
                //{
                //    Token = "randomToken",
                //    Document = new APLDocument { MainTemplate = mainLayout, Styles = new Dictionary<string, Style>() {
                //        { "baseText", style}
                //    } }
                //};

                // Build a response to combine both speech & APL responses
                var response = ResponseBuilder.Tell(output);
                //response.Response.Directives.Add(renderDocument);

                return response;
            }

            // If display is not available
            else
            {
                logger.LogLine($"[LaunchRequestHandler.HandleRequest()] Generating speech only response");

                var output = SsmlBuilder.LaunchSpeech(hasDisplay);

                // Return speech response only
                return ResponseBuilder.Ask(output, null);
            }
        }


        public async Task<SkillResponse> HandleRequest(IntentRequest request, Session session, ILambdaLogger logger)
        {
            throw new NotImplementedException();
        }


        public async Task<SkillResponse> HandleRequest(AudioPlayerRequest request, Session session, ILambdaLogger logger)
        {
            throw new NotImplementedException();
        }
    }
}
