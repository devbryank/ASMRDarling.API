using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Alexa.NET.Response.APL;
using Alexa.NET.APL.Components;
using ASMRDarling.API.Interfaces;

namespace ASMRDarling.API.Handlers
{
    class LaunchRequestHandler : ILaunchRequestHandler
    {
        public LaunchRequestHandler() { }


        public async Task<SkillResponse> HandleRequest(LaunchRequest request, ILambdaLogger logger)
        {
            logger.LogLine($"[LaunchRequestHandler.HandleRequest()] Launch request handling started");

            var output = new SsmlOutputSpeech()
            {
                Ssml = "<speak>" +
                            "<amazon:effect name='whispered'>" +
                                 "<prosody rate='slow'>" +
                                      "<p>Hey,</p>" +
                                      "<p>it's me.</p>" +
                                      "<p>ASMR Darling.</p>" +
                                      "<p>To begin,</p>" +
                                      "<p>you can say things like,</p>" +
                                      "<p>play 10 triggers to help you sleep,</p>" +
                                      "<p>or just say play 10 triggers.</p>" +
                                 "</prosody>" +
                            "</amazon:effect>" +
                       "</speak>"
            };


            // image arrays?

            // var directive = new RenderDocumentDirective
            // {
            //     Token = "randomToken",
            //     Document = new APLDocument
            //     {
            //         MainTemplate = new Layout(new[]
            //{
            //             new Container(new APLComponent[]{
            //                 new Text("APL in C#"){FontSize = "24dp",TextAlign= "Center"},
            //                 new Image("https://images.example.com/photos/2143/lights-party-dancing-music.jpg?cs=srgb&dl=cheerful-club-concert-2143.jpg&fm=jpg"){Width = 400,Height=400}
            //             }){Direction = "row"}
            //         })
            //     }
            // };

            //var sentences = "Hello World!";
            //var mainLayout = new Layout(
            //    new Container(
            //        new ScrollView(
            //            new Text(sentences)
            //            {
            //                FontSize = "60dp",
            //                TextAlign = "Center",
            //                Id = "talker"
            //            }
            //            )
            //        { Width = "50vw", Height = "100vw" }
            //            ));



            var mainLayout = new Layout(
                                 new[] {
                                     new Container (
                                         new Sequence(
                                                 new APLComponent[] {
                                                     new Text("Video List") { FontSize = "24dp", TextAlign = "Center" },
                                                     new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/100triggerstohelpyousleep.png") { Width = 200 , Height = 200},
                                                     new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/10triggerstohelpyousleep.png") { Width = 200, Height = 200},
                                                     new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/20triggerstohelpyousleep.png") { Width = 200, Height = 200},
                                                     new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/atoztriggerstohelpyousleep.png") { Width = 200 , Height = 200},
                                                     new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/10triggerstohelpyousleep.png") { Width = 200 , Height = 200},
                                                     new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/10triggerstohelpyousleep.png") { Width = 200,Height = 200 },
                                                     new Image("https://s3.amazonaws.com/asmr-darling-api-media/png/10triggerstohelpyousleep.png") { Width = 200, Height = 200 }

                                                 }
                                                 ){Height="100vh",Numbered=true,  AlignSelf="end"}

                                     ){}
                                 }
                  );



            //var shape = input.Context.Viewport?.Shape;
            //var response = ResponseBuilder.Tell($"Your viewport is {shape.ToString() ?? "Non existent"}");

            var response = ResponseBuilder.Tell(output);
            var renderDocument = new RenderDocumentDirective
            {
                Token = "randomToken",
                Document = new APLDocument
                {
                    MainTemplate = mainLayout
                }
            };

            response.Response.Directives.Add(renderDocument);
            return response;


            //return ResponseBuilder.Ask(output, null);
        }


        public async Task<SkillResponse> HandleRequest(IntentRequest request, Session session, ILambdaLogger logger)
        {
            throw new NotImplementedException();
        }


        public async Task<SkillResponse> HandleRequest(AudioPlayerRequest request, ILambdaLogger logger)
        {
            throw new NotImplementedException();
        }
    }
}
