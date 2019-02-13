using System;
using System.Threading.Tasks;

using Amazon.Lambda.Core;

using Alexa.NET;
using Alexa.NET.Response;

namespace ASMRDarling.API.Helpers
{
    /// <summary>
    /// This class processes the incoming request from Alexa with
    /// an appropriate exception handling & logging method
    /// </summary>
    class RequestProcessor
    {
        public static async Task<SkillResponse> ProcessAlexaRequest(string funcName, string taskName, Func<Task<SkillResponse>> handler, ILambdaLogger logger)
        {
            logger.LogLine($"[{funcName}] {taskName} handling started");

            SkillResponse response = new SkillResponse();

            try
            {
                logger.LogLine($"[{funcName}] Processing {taskName} in progress");
                response = handler();
                logger.LogLine($"[{funcName}] Processing {taskName} completed");
            }
            catch (Exception ex)
            {
                logger.LogLine($"[{funcName}] Unable to process the request");
                logger.LogLine($"[{funcName}] Exception details: {ex}");

                response = ResponseBuilder.Tell("error");
            }

            return response;
        }
    }
}
