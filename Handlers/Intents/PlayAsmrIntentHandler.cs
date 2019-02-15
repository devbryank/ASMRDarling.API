using System.Threading.Tasks;
using System.Text.RegularExpressions;

using Amazon.Lambda.Core;

using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Alexa.NET.Response.Directive;
using ASMRDarling.API.Helpers;
using ASMRDarling.API.Templates;
using ASMRDarling.API.Interfaces;
using ASMRDarling.API.Models;

namespace ASMRDarling.API.Handlers
{
    /// <summary>
    /// This class processes the play ASMR intent request
    /// </summary>
    class PlayAsmrIntentHandler : IPlayAsmrIntentHandler
    {
        public Task<SkillResponse> HandleIntent(Intent intent, MediaState currentState, Session session, ILambdaLogger logger)
        {
            return RequestProcessor.ProcessAlexaRequest("PlayAsmrIntentHandler.HandleIntent()", "Play ASMR Intent", async () =>
            {
                SkillResponse response = new SkillResponse();


                // Save user state
                //session.Attributes["user_state"] = UserStates.Media;
                //string userState = session.Attributes["user_state"] as string;
                //logger.LogLine($"[LaunchRequestHandler.HandleRequest()] User state updated to: {userState}");


                // Get slot values
                Slot slot = intent.Slots[AlexaRequestConstants.MediaItemSlot];
                string slotValue = slot.Value;
                logger.LogLine($"[PlayAsmrIntentHandler.HandleIntent()] Requested slot value (synonym): {slotValue}");


#warning resolution should support multiple slots

                // Get resolution, multiple slots in a dialog will cause an exception
                ResolutionAuthority[] resolution = slot.Resolution.Authorities;
                ResolutionValueContainer[] container = resolution[0].Values;
                string mediaTitle = container[0].Value.Name;


                // Get file extentions based on the display availability
                bool? hasDisplay = session.Attributes["has_display"] as bool?;
                string fileType = hasDisplay == true ? "mp4" : "mp3";


                // Store session state for a title
                session.Attributes["current_video_item"] = mediaTitle;
                logger.LogLine($"[PlayAsmrIntentHandler.HandleIntent()] Session state for the current video item: {mediaTitle}");


                // Convert file name into lower cases with no white spaces
                var fileName = Regex.Replace(mediaTitle, @"\s", "").ToLower();
                logger.LogLine($"[PlayAsmrIntentHandler.HandleIntent()] Media file requested: {fileName}.{fileType}");


                // Get file source url
                string url = UrlBuilder.GetS3FileSourceUrl(fileName, fileType);
                logger.LogLine($"[PlayAsmrIntentHandler.HandleIntent()] Media file source URL: {url}");


                // Send a quick response before loading the content
                ProgressiveResponse progressiveResponse = session.Attributes["quick_response"] as ProgressiveResponse;
                await progressiveResponse.SendSpeech($"Playing {mediaTitle}");


                // Set response
                if (hasDisplay == true)
                {
                    logger.LogLine($"[PlayAsmrIntentHandler.HandleIntent()] Generating a video app or APL video player response");

#warning video app or apl video? which one is better?

                    //set video app response
                    //response = ResponseBuilder.Empty();
                    //response.Response.Directives.Add(new VideoAppDirective(url));

                    // Set APL video player response
                    return await AplTemplate.VideoPlayer(response, url);
                }
                else
                {
                    logger.LogLine($"[PlayAsmrIntentHandler.HandleIntent()] Generating an audio player response");
                    currentState.State.State = "PLAY_MODE";
                    currentState.State.Token = fileName;
                    currentState.State.Index = MediaItems.GetMediaItems().Find(m => m.FileName.Equals(fileName)).Id;

                    response = ResponseBuilder.AudioPlayerPlay(PlayBehavior.ReplaceAll, url, currentState.State.Token);
                }

                // Return response
                response.SessionAttributes = session.Attributes;
                return response;

            }, logger);
        }
    }
}
