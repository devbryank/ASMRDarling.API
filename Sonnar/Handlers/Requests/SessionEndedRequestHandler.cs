using System.Threading.Tasks;

using Sonnar.Helpers;
using Sonnar.Components;
using Sonnar.Interfaces;

namespace Sonnar.Handlers
{
    class SessionEndedRequestHandler : IRequestHandler
    {
        public async Task HandleRequest()
        {
            await RequestProcessHelper.ProcessRequest("SessionEndedRequestHandler.HandleRequest()", "Session Ended Request", async () =>
            {
                Core.Logger.Write("SessionEndedRequestHandler.HandleRequest()", "Session eneded");
            });
        }
    }
}
