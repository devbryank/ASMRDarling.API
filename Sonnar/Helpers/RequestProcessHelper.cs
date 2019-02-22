using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

using Sonnar.Templates;
using Sonnar.Components;

namespace Sonnar.Helpers
{
    class RequestProcessHelper
    {
        public static async Task ProcessRequest(string funcName, string taskName, Func<Task> handler)
        {
            try
            {
                Core.Logger.Write($"{funcName}", $"Processing {taskName} started");
                await handler();
                Core.Logger.Write($"{funcName}", $"Processing {taskName} completed");
            }
            catch (Exception ex)
            {
                Core.Logger.Write($"{funcName}", $"Unable to process {taskName}");
                Core.Logger.Write($"{funcName}", $"Exception details: {JsonConvert.SerializeObject(ex)}");
                Core.Response.SetSpeech(false, true, SpeechTemplate.SystemException);
            }
        }
    }
}
