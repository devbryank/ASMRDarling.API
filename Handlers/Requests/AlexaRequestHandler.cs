using Amazon.Lambda.Core;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Alexa.NET.Response.Directive;
using Alexa.NET.APL;
using ASMRDarling.API.Interfaces;
using ASMRDarling.API.Templates;

namespace ASMRDarling.API.Handlers
{
    class AlexaRequestHandler : IAlexaRequestHandler
    {
        // Request names
        const string UserEventRequestName = "UserEvent";


        // Constructor
        public AlexaRequestHandler() { }


        // Request handler start
        public async Task<SkillResponse> HandleRequest(SkillRequest input, Session session, ILambdaLogger logger)
        {
            logger.LogLine($"[AlexaRequestHandler.HandleRequest()] Alexa request handling started");

            // Split request name to get the suffix only
            var requestNamePartials = input.Request.Type.ToString().Split('.');
            string suffix = requestNamePartials[requestNamePartials.Length - 1];

            // Declare response to return
            SkillResponse response = new SkillResponse();

            // Direct intent into the matching handler
            switch (suffix)
            {
                // Handle user event request
                case UserEventRequestName:

                    // Get user event request
                    logger.LogLine($"[AlexaRequestHandler.HandleRequest()] Directing request into {suffix} handler");
                    UserEventRequest userEventRequest = input.Request as UserEventRequest;

                    string url = userEventRequest.Arguments[0] as string;
                    logger.LogLine($"[AlexaRequestHandler.HandleRequest()] Media file source URL: {url}");
                    logger.LogLine($"[AlexaRequestHandler.HandleRequest()] Generating a video app or APL video player response");

                    //// Set video app response
                    //response = ResponseBuilder.Empty();
                    //response.Response.Directives.Add(new VideoAppDirective(url));

                    // Set apl video player response
                    response = ResponseBuilder.Empty();
                    response = await AplTemplate.VideoPlayer(response, url);
                    break;

                // Handle default case
                default:

#warning not implemented yet

                    break;
            }

            // Return response to the function handler
            return response;
        }
    }
}
