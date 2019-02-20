using System.Text;
using System.Threading.Tasks;

using Sonnar.Helpers;
using Sonnar.Templates;
using Sonnar.Components;
using Sonnar.Interfaces;

namespace Sonnar.Handlers
{
    class LaunchRequestHandler : IRequestHandler
    {
        public async Task HandleRequest()
        {
            await RequestProcessHelper.ProcessRequest("LaunchRequestHandler.HandleRequest()", "Launch Request", () =>
            {
                // set user state to menu
                Core.State.UserState.State = "MENU_MODE";
                Core.Logger.Write("LaunchRequestHandler.HandleRequest()", $"User state updated to: {Core.State.UserState.State}");

                // build speech response
                StringBuilder sb = new StringBuilder();
                string text = Core.State.UserState.NumTimesPlayed > 0 ? SpeechTemplate.WelcomeBack : SpeechTemplate.WelcomeNew;
                string intro = SpeechTemplate.Intro;

                sb.Append(text);
                sb.Append(intro);

                // increment play count
                Core.State.UserState.NumTimesPlayed++;
                Core.Response.SetAskSpeech(sb.ToString());

                // compose apl response if has display
                if (Core.Device.HasScreen)
                {
                    Core.Logger.Write("LaunchRequestHandler.HandleRequest()", "Generating visual response for display interfaces");

                    if (Core.Device.IsRound)
                        Core.Response.AddAplPage("SpotAplMenu", AplTemplate.GetSpotMenu());
                    else
                        Core.Response.AddAplPage("ShowAplMenu", AplTemplate.GetShowMenu());
                }

                Core.Logger.Write("LaunchRequestHandler.HandleRequest()", "Initial response generated");
                return Task.CompletedTask;
            });
        }
    }
}
