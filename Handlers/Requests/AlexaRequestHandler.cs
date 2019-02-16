using Amazon.Lambda.Core;
using System.Threading.Tasks;
using System.Collections.Generic;
using Alexa.NET;
using Alexa.NET.APL;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Alexa.NET.Response.Directive;
using ASMRDarling.API.Data;
using ASMRDarling.API.Models;
using ASMRDarling.API.Helpers;
using ASMRDarling.API.Interfaces;

namespace ASMRDarling.API.Handlers
{
    class AlexaRequestHandler : IAlexaRequestHandler
    {
        public Task<SkillResponse> HandleRequest(SkillRequest input, MediaState currentState, Session session, ILambdaLogger logger)
        {
            return RequestProcessHelper.ProcessAlexaRequest("[AlexaRequestHandler.HandleRequest()]", "Alexa Reqeust", async () =>
            {
                SkillResponse response = new SkillResponse();
                response = ResponseBuilder.Empty();
                //string subIntentType = session.Attributes["sub_intent"] as string;
                string subIntentType = AlexaRequestConstants.UserEvent;

                // direct request into an appropriate handler
                switch (subIntentType)
                {
                    // handle user event request
                    case AlexaRequestConstants.UserEvent:
                        logger.LogLine($"[AlexaRequestHandler.HandleRequest()] Directing request into {AlexaRequestConstants.UserEvent} handler");

                        UserEventRequest userEventRequest = input.Request as UserEventRequest;

                        string title = userEventRequest.Arguments[0] as string;
                        string url = userEventRequest.Arguments[1] as string;

                        logger.LogLine($"[AlexaRequestHandler.HandleRequest()] Media file source URL: {url}");
                        logger.LogLine($"[AlexaRequestHandler.HandleRequest()] Session state for the current video item: {title}");
                        logger.LogLine($"[AlexaRequestHandler.HandleRequest()] Generating a video app or APL video player response");
                        //response.Response = new ResponseBody();
                        // set video app response
                        VideoAppDirective videoDirective = new VideoAppDirective
                        {
                            VideoItem = new VideoItem(url)
                        };

                        videoDirective.VideoItem.Metadata = new VideoItemMetadata
                        {
                            Title = "Sample Title",
                            Subtitle = "No subtitles yet"
                        };

                        response.Response.Directives = new List<IDirective>
                        {
                            videoDirective
                        };

                        response.Response.ShouldEndSession = null;
                        break;


                    // handle default fallback case
                    default:

#warning not implemented yet

                        break;
                }


                // return response back to function handler
                return response;

            }, logger);
        }
    }
}
