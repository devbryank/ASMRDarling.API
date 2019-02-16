using Amazon.Lambda.Core;
using System;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Response;
using ASMRDarling.API.Data;

namespace ASMRDarling.API.Helpers
{
    /// <summary>
    /// request process wrapper to log exceptions
    /// </summary>
    class RequestProcessHelper
    {
        public static async Task<SkillResponse> ProcessAlexaRequest(string funcName, string taskName, Func<Task<SkillResponse>> handler, ILambdaLogger logger)
        {
            SkillResponse response = new SkillResponse();
            logger.LogLine($"[{funcName}] {taskName} handling started");

            try
            {
                logger.LogLine($"[{funcName}] Processing {taskName} in progress");
                response = await handler();
                logger.LogLine($"[{funcName}] Processing {taskName} completed");
            }
            catch (Exception ex)
            {
                logger.LogLine($"[{funcName}] Unable to process the request");
                logger.LogLine($"[{funcName}] Exception details: {ex}");

                var output = SsmlTemplate.FatalExceptionSpeech();
                response = ResponseBuilder.Tell(output);
            }

            return response;
        }
    }
}
