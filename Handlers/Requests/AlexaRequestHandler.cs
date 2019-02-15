using Alexa.NET;
using Alexa.NET.APL;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Alexa.NET.Response.Directive;
using Amazon.Lambda.Core;
using ASMRDarling.API.Helpers;
using ASMRDarling.API.Interfaces;
using ASMRDarling.API.Models;
using ASMRDarling.API.Templates;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ASMRDarling.API.Handlers
{
    class AlexaRequestHandler : IAlexaRequestHandler
    {
        public Task<SkillResponse> HandleRequest(SkillRequest input, MediaState currentState, Session session, ILambdaLogger logger)
        {
            return RequestProcessor.ProcessAlexaRequest("[AlexaRequestHandler.HandleRequest()]", "Alexa Reqeust", async () =>
            {


                // split request name to get the suffix only
                var requestNamePartials = input.Request.Type.ToString().Split('.');
                string suffix = requestNamePartials[requestNamePartials.Length - 1];

                // declare response to return
                SkillResponse response = new SkillResponse();

                // direct request into the matching handler
                switch (suffix)
                {
                    // handle user event request
                    case AlexaConstants.UserEvent:
                        logger.LogLine($"[AlexaRequestHandler.HandleRequest()] Directing request into {AlexaConstants.UserEvent} handler");

                        // get user event request
                        UserEventRequest userEventRequest = input.Request as UserEventRequest;

                        string title = userEventRequest.Arguments[0] as string;
                        string url = userEventRequest.Arguments[1] as string;
                        //session.Attributes["current_video_item"] = title;

                        logger.LogLine($"[AlexaRequestHandler.HandleRequest()] Media file source URL: {url}");
                        logger.LogLine($"[AlexaRequestHandler.HandleRequest()] Session state for the current video item: {title}");
                        logger.LogLine($"[AlexaRequestHandler.HandleRequest()] Generating a video app or APL video player response");

                        //set video app response
                        response = ResponseBuilder.Empty();
                        VideoAppDirective videoDirective = new VideoAppDirective();
                        videoDirective.VideoItem = new VideoItem(url);
                        videoDirective.VideoItem.Metadata = new VideoItemMetadata();
                        videoDirective.VideoItem.Metadata.Title = "Sample Title";
                        videoDirective.VideoItem.Metadata.Subtitle = "No subtitles yet";
                        response.Response.Directives = new List<IDirective>();


                        response.Response.Directives.Add(videoDirective);
                        response.Response.ShouldEndSession = null;

                        // set apl video player response (autoplay = false, shouldendsession = false)
                        //response = ResponseBuilder.Empty();
                        //response.Response.ShouldEndSession = false;
                        //response = await AplTemplate.VideoPlayer(response, url);
                        break;

                    // handle default case
                    default:

#warning not implemented yet

                        break;
                }

                // return response to the function handler
                return response;




            }, logger);
        }
    }
}
