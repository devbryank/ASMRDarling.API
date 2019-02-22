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
                // set user stage
                Core.State.UserState.Stage = Stage.Menu;


                // compose output speech
                StringBuilder sb = new StringBuilder();
                string welcome = Core.State.UserState.NumTimesPlayed > 0 ? SpeechTemplate.WelcomeBack : SpeechTemplate.WelcomeNew;


                sb.Append(welcome);

                string intro = SpeechTemplate.Intro;


                if (Core.State.UserState.NumTimesPlayed == 5 || Core.State.UserState.NumTimesPlayed == 10)
                {
                    sb.Append(AskForReview());
                }
                else
                {
                    sb.Append(intro);
                }

                //sb.Append(welcome + intro);


                // increment play count
                Core.State.UserState.NumTimesPlayed++;
                Core.Response.SetSpeech(false, false, sb.ToString());


                // set apl response if has display
                if (Core.Device.HasScreen)
                {
                    Core.Logger.Write("LaunchRequestHandler.HandleRequest()", "Generating visual response for display interface");

                    if (Core.Device.IsRound)
                        Core.Response.AddAplPage("SpotAplMenu", AplTemplate.GetSpotMenu()); // spot device
                    else
                        Core.Response.AddAplPage("ShowAplMenu", AplTemplate.GetShowMenu()); // show device
                }


                return Task.CompletedTask;
            });
        }

        string AskForReview()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Sounds like you enjoy our skill. ");
            //sb.Append(" If you wish to keep using it for free, all it takes is a 5 stars review. ");
            sb.Append(" Search for ASMR Daring in your alexa app and select write a review. ");
            if (Core.State.UserState.NumTimesPlayed == 5)
            {
                sb.Append(" Don't worry, I am going to remind you about this only one more time. ");
            }
            else
            {
                sb.Append("Don't worry, this is the last time you hear this reminder. ");
            }
            return sb.ToString();

        }
    }
}
