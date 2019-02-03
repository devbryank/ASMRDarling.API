using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Alexa.NET.Response.Directive;
using Alexa.NET.APL;
using ASMRDarling.API.Templates;
using ASMRDarling.API.Interfaces;

namespace ASMRDarling.API.Handlers
{
    class AlexaRequestHandler : IAlexaRequestHandler
    {
        // request suffix constants
        const string UserEventRequestSuffix = "UserEvent";


        public AlexaRequestHandler() { }

        public async Task<SkillResponse> HandleRequest(SkillRequest input, Session session, ILambdaLogger logger)
        {
            try
            {
                logger.LogLine($"[AlexaRequestHandler.HandleRequest()] Alexa Request handling started");

                // split request name to get the suffix only
                var requestNamePartials = input.Request.Type.ToString().Split('.');
                string suffix = requestNamePartials[requestNamePartials.Length - 1];

                // declare response to return
                SkillResponse response = new SkillResponse();

                // direct request into the matching handler
                switch (suffix)
                {
                    // handle user event request
                    case UserEventRequestSuffix:
                        logger.LogLine($"[AlexaRequestHandler.HandleRequest()] Directing request into {UserEventRequestSuffix} handler");

                        // get user event request
                        UserEventRequest userEventRequest = input.Request as UserEventRequest;

                        string url = userEventRequest.Arguments[0] as string;
                        logger.LogLine($"[AlexaRequestHandler.HandleRequest()] Media file source URL: {url}");
                        logger.LogLine($"[AlexaRequestHandler.HandleRequest()] Generating a video app or APL video player response");

                        // set video app response
                        // response = ResponseBuilder.Empty();
                        // response.Response.Directives.Add(new VideoAppDirective(url));

                        // set apl video player response
                        response = ResponseBuilder.Empty();
                        response = await AplTemplate.VideoPlayer(response, url);
                        break;

                    // handle default case
                    default:

#warning not implemented yet

                        break;
                }

                // return response to the function handler
                return response;
            }
            catch (Exception ex)
            {
                logger.LogLine($"[AlexaRequestHandler.HandleRequest()] Exception caught");
                logger.LogLine($"[AlexaRequestHandler.HandleRequest()] Exception log: {ex}");

                // return system exception to the function handler
                var output = SsmlTemplate.SystemFaultSpeech();
                return ResponseBuilder.Tell(output);
            }
        }
    }
}
