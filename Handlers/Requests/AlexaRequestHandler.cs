using Amazon.Lambda.Core;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Alexa.NET.APL;
using ASMRDarling.API.Interfaces;
using Alexa.NET;
using Alexa.NET.Response.Directive;

namespace ASMRDarling.API.Handlers
{
    class AlexaRequestHandler : IAlexaRequestHandler
    {
        // Request names
        const string UserEventRequestName = "UserEvent";


        // Constructor
        public AlexaRequestHandler() { }


        // Request handler
        public async Task<SkillResponse> HandleRequest(SkillRequest input, Session session, ILambdaLogger logger)
        {
            logger.LogLine($"[AlexaRequestHandler.HandleRequest()] Alexa request handling started");

            // Split request name to get the suffix only
            var requestNamePartials = input.Request.GetType().Name.Split('.');
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
                    logger.LogLine($"[AlexaRequestHandler.HandleIntent()] Media file source URL: {url}");

                    // Return video app response
                    response = ResponseBuilder.Empty();
                    response.Response.Directives.Add(new VideoAppDirective(url));
                    break;

                default:
                    break;
            }

            return response;
        }
    }
}
